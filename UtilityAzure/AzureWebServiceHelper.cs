﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using Utility;
using System.IO;
using System.Management.Automation.Runspaces;

namespace UtilityAzure
{
    public class AzureWebServiceHelper
    {
        public static string StartVMScriptPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\UtilityAzure\\PowerShellScripts\\startVM_az.ps1"));
        public static string StopVMScriptPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\UtilityAzure\\PowerShellScripts\\stopVM_az.ps1"));
        public static HostedRunspace hr;

        public AzureWebServiceHelper()
        {
            hr = new HostedRunspace();
            hr.InitializeRunspaces(1, 5, new string[0] {});
        }

        public void StartVM()
        {
            string scriptContents = FileHelper.ReadFile(StartVMScriptPath);
            hr.RunScript(scriptContents, new Dictionary<string, object>());
        }

        public void StopVM()
        {
            string scriptContents = FileHelper.ReadFile(StopVMScriptPath);
            hr.RunScript(scriptContents, new Dictionary<string, object>());
        }
    }

    public class HostedRunspace
    {
        /// <summary>
        /// The PowerShell runspace pool.
        /// </summary>
        private RunspacePool RsPool { get; set; }

        /// <summary>
        /// Initialize the runspace pool.
        /// </summary>
        /// <param name="minRunspaces"></param>
        /// <param name="maxRunspaces"></param>
        public void InitializeRunspaces(int minRunspaces, int maxRunspaces, string[] modulesToLoad)
        {
            // create the default session state.
            // session state can be used to set things like execution policy, language constraints, etc.
            // optionally load any modules (by name) that were supplied.

            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            foreach (var moduleName in modulesToLoad)
            {
                defaultSessionState.ImportPSModule(moduleName);
            }

            // use the runspace factory to create a pool of runspaces
            // with a minimum and maximum number of runspaces to maintain.

            RsPool = RunspaceFactory.CreateRunspacePool(defaultSessionState);
            RsPool.SetMinRunspaces(minRunspaces);
            RsPool.SetMaxRunspaces(maxRunspaces);

            // set the pool options for thread use.
            // we can throw away or re-use the threads depending on the usage scenario.

            RsPool.ThreadOptions = PSThreadOptions.UseNewThread;

            // open the pool. 
            // this will start by initializing the minimum number of runspaces.

            RsPool.Open();
        }

        /// <summary>
        /// Runs a PowerShell script with parameters and prints the resulting pipeline objects to the console output. 
        /// </summary>
        /// <param name="scriptContents">The script file contents.</param>
        /// <param name="scriptParameters">A dictionary of parameter names and parameter values.</param>
        public void RunScript(string scriptContents, Dictionary<string, object> scriptParameters)
        {
            if (RsPool == null)
            {
                throw new ApplicationException("Runspace Pool must be initialized before calling RunScript().");
            }

            // create a new hosted PowerShell instance using a custom runspace.
            // wrap in a using statement to ensure resources are cleaned up.

            using (PowerShell ps = PowerShell.Create())
            {
                // use the runspace pool.
                ps.RunspacePool = RsPool;

                // specify the script code to run.
                ps.AddScript(scriptContents);

                // specify the parameters to pass into the script.
                ps.AddParameters(scriptParameters);

                // subscribe to events from some of the streams
                ps.Streams.Error.DataAdded += Error_DataAdded;
                ps.Streams.Warning.DataAdded += Warning_DataAdded;
                ps.Streams.Information.DataAdded += Information_DataAdded;

                // execute the script and await the result.
                try
                {
                    var pipelineObjects = ps.InvokeAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    // print the resulting pipeline objects to the console.
                    //Console.WriteLine("----- Pipeline Output below this point -----");
                    //foreach (var item in pipelineObjects)
                    //{
                    //    Console.WriteLine(item.BaseObject.ToString());
                    //}
                    Console.WriteLine("Executed Powershell script.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an error during execution of Powershell script.");
                    throw e;
                }
            }
        }

        /// <summary>
        /// Handles data-added events for the information stream.
        /// </summary>
        /// <remarks>
        /// Note: Write-Host and Write-Information messages will end up in the information stream.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<InformationRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            Console.WriteLine($"InfoStreamEvent: {currentStreamRecord.MessageData}");
        }

        /// <summary>
        /// Handles data-added events for the warning stream.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<WarningRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            Console.WriteLine($"WarningStreamEvent: {currentStreamRecord.Message}");
        }

        /// <summary>
        /// Handles data-added events for the error stream.
        /// </summary>
        /// <remarks>
        /// Note: Uncaught terminating errors will stop the pipeline completely.
        /// Non-terminating errors will be written to this stream and execution will continue.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<ErrorRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            Console.WriteLine($"ErrorStreamEvent: {currentStreamRecord.Exception}");
        }
    }

}
