using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using Jpp.Common;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.DataTypes;
using TLS.DesignLibrary.Engine.Project;

namespace TLS.DesignLibrary.Engine
{
    public class CalculationContainer : BaseNotify
    {
        [JsonInclude]
        public List<EngineCalculation> ContextlessCalculations
        {
            get { return _contextlessCalculations; }
            private set { _contextlessCalculations = value; }
        }

        private List<EngineCalculation> _contextlessCalculations;

        [JsonInclude]
        public List<EngineCalculation> ContextualCalculations
        {
            get { return _contextualCalculations; }
            private set { _contextualCalculations = value; }
        }
        
        [JsonInclude]
        public ObservableCollection<LoadCase> LoadCases { get; set; }
        
        [JsonInclude]
        public ObservableCollection<Combination> Combinations { get; set; }
        
        [JsonInclude]
        public Combination? ActiveCombinationGroup { get; set; }
        
        [JsonInclude]
        public List<ActiveCombinationGroup> ActiveCombinationGroups { get; set; }

        public IReadOnlyList<EngineCalculation> Calculations
        {
            get
            {
                List<EngineCalculation> result = new List<EngineCalculation>();
                result.AddRange(ContextlessCalculations);
                result.AddRange(ContextualCalculations);
                return result;
            }
        }
        
        public string Output { get; set; }

        private List<EngineCalculation> _contextualCalculations;

        public CalculationContainer()
        {
            _contextlessCalculations = new List<EngineCalculation>();
            _contextualCalculations = new List<EngineCalculation>();

            ActiveCombinationGroups = new List<ActiveCombinationGroup>();
            ActiveCombinationGroup = null;
            
            LoadCases = new ObservableCollection<LoadCase>()
            {
                new LoadCase()
                {
                    Id = Guid.Parse("66d7ee45-c8b2-40b8-9e7d-defa42eef48f"),
                    Name = "Self-Weight",
                    Type = LoadCaseType.Permanent
                }
            };
            Combinations = new ObservableCollection<Combination>()
            {
                new Combination()
                {
                    Id = Guid.Parse("1c9efd78-82ef-46b4-a4c1-626e4e857e0b"),
                    Name = "ULS",
                    CombinationType = CombinationType.ULS_EQ,
                    LoadFactor = new Dictionary<Guid, double>
                    {
                        { Guid.Parse("66d7ee45-c8b2-40b8-9e7d-defa42eef48f"), 1.35 }
                    }
                }
            };
            
            LoadCases.CollectionChanged += LoadCasesOnCollectionChanged;
            Combinations.CollectionChanged += CombinationsOnCollectionChanged;
        }

        private void CombinationsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(Combinations));
            //TODO: Implement
        }

        private void LoadCasesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(LoadCases));
            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Combination combination in Combinations)
                {
                    foreach (LoadCase loadCase in e.NewItems)
                    {
                        combination.LoadFactor.Add(loadCase.Id, 0);
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Combination combination in Combinations)
                {
                    foreach (LoadCase loadCase in e.OldItems)
                    {
                        combination.LoadFactor.Remove(loadCase.Id);
                    }
                }
            }
        }

        public Calculation AddCalculation(Type info, double x, double y, string name, IUnitConverter converter)
        {
            Calculation calc = (Calculation)Activator.CreateInstance(info);
            EngineCalculation eCalc = new EngineCalculation(x, y, name, calc, converter);
            eCalc.PropertyChanged += (sender, args) => { OnPropertyChanged(nameof(ContextlessCalculations)); };
            if (typeof(ContextualCalculation).IsAssignableFrom(calc.GetType()))
            {
                _contextualCalculations.Add(eCalc);
                OnPropertyChanged(nameof(ContextualCalculations));
            }
            else
            {
                _contextlessCalculations.Add(eCalc);
                OnPropertyChanged(nameof(ContextlessCalculations));
            }

            return calc;
        }

        //TODO: Should this have a converter?
        public void OnDeserialize(IUnitConverter converter)
        {
            foreach (EngineCalculation calc in Calculations)
            {
                calc.PropertyChanged += (sender, args) => this.OnPropertyChanged(nameof(Calculations));
                calc.OnDeserialize(converter);
            }
            
            LoadCases.CollectionChanged += LoadCasesOnCollectionChanged;
            Combinations.CollectionChanged += CombinationsOnCollectionChanged;
        }
    }
}
