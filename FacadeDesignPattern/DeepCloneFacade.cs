using PrototypeDesignPattern;
using StateAndDecoratorDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacadeDesignPattern
{
    public class DeepCloneFacade
    {
        private List<SignalModelContext> _collectionOfSignalsToBeCloned { get; set; }
        private Object _padlock { get; set; }

        public DeepCloneFacade()
        {
            _padlock = new object();
        }

        public void ChangeClonnedCollection(List<SignalModelContext> collectionOfSignalsToBeCloned)
        {
            _collectionOfSignalsToBeCloned = collectionOfSignalsToBeCloned;
        }

        private List<SignalModelContext> CloneCollectionUsingJsonOrBinarySerialization()
        {
            List<SignalModelContext> clonedSignalContextCollection = new List<SignalModelContext>();

            Parallel.ForEach(_collectionOfSignalsToBeCloned, (signal) =>
            {
                SignalModelContext singleClonedSignalContext = signal.Clone() as SignalModelContext;

                lock (_padlock)
                {
                    clonedSignalContextCollection.Add(singleClonedSignalContext);
                }
            });

            clonedSignalContextCollection = clonedSignalContextCollection.OrderBy(z => z.Date).ToList();
            return clonedSignalContextCollection;
        }

        public List<SignalModelContext> DeepCloneUsingJsonSerialization()
        {
            _collectionOfSignalsToBeCloned.AsParallel().ForAll(z => z.JsonSerialization = true);
            return CloneCollectionUsingJsonOrBinarySerialization();
        }

        public List<SignalModelContext> DeepCloneUsingBinarySerialization()
        {
            _collectionOfSignalsToBeCloned.AsParallel().ForAll(z => z.JsonSerialization = false);
            return CloneCollectionUsingJsonOrBinarySerialization();
        }

        public List<SignalModelContext> DeepCloneUsingReflection()
        {
            List<SignalModelContext> clonedSignalContextCollection = new List<SignalModelContext>();

            Parallel.ForEach(_collectionOfSignalsToBeCloned, (signal) =>
            {
                SignalModelContext singleClonedSignalContext = ReflectionDeepCopy.CloneObject(signal) as SignalModelContext;

                lock (_padlock)
                {
                    clonedSignalContextCollection.Add(singleClonedSignalContext);
                }
            });

            clonedSignalContextCollection = clonedSignalContextCollection.OrderBy(z => z.Date).ToList();
            return clonedSignalContextCollection;
        }
    }
}
