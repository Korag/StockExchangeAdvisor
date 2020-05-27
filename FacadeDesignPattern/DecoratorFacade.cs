using DecoratorDesignPattern;
using StateAndDecoratorDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace FacadeDesignPattern
{
    public class DecoratorFacade
    {
        public DecoratorFacade()
        {

        }

        public List<DecoratorComponent> GetAllDecoratorsTypes()
        {
            var abstractClass = typeof(DecoratorComponent);

            var derivedClass = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(z => z.GetTypes())
                .Where(z => abstractClass.IsAssignableFrom(z)
                && !z.IsInterface && !z.IsAbstract && z.Name != "DecoratorConcreteComponent")
                .ToList();

            List<DecoratorComponent> decoratorsTypes = new List<DecoratorComponent>();

            foreach (var type in derivedClass)
            {
                DecoratorComponent decorator = (DecoratorComponent)Activator.CreateInstance(type);
                decoratorsTypes.Add(decorator);
            }

            return decoratorsTypes;
        }

        public void CalculateAdditionalFeeAndFinalPrice(ref List<SignalModelContext> signalWithQuotessignalsWithQuotes, List<DecoratorComponent> decoratorsType = null)
        {
            if (decoratorsType == null)
            {
                decoratorsType = GetAllDecoratorsTypes();
            }

            Parallel.ForEach(signalWithQuotessignalsWithQuotes, (quoteWSignals) =>
            {
                double fee = 0;
                double finalPrice = 0;

                DecoratorComponent decorator = new DecoratorConcreteComponent();
                decorator = AutoMapperHelper.MapQuotesAndSignalsToDecoratorObject(quoteWSignals);

                finalPrice = decorator.CalculateCost();

                foreach (var decoratorType in decoratorsType)
                {
                    decorator = (DecoratorComponent)Activator.CreateInstance(decoratorType.GetType(), decorator);
                    finalPrice = decorator.CalculateCost();
                    fee += decorator.CalculateAdditionalFee();
                }

                double formattedFee;
                double formattedFinalPrice;

                double.TryParse(String.Format("{0:0.##}", fee), out formattedFee);
                double.TryParse(String.Format("{0:0.##}", finalPrice), out formattedFinalPrice);

                quoteWSignals.AdditionalFee = formattedFee;
                quoteWSignals.FinalPrice = formattedFinalPrice;
            });
        }
    }
}
