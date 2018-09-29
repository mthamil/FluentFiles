namespace FluentFiles.Core.Base
{
    using System.Collections.Generic;
    using System.Linq;

    public class AutoOrderedFieldsContainer<TFieldSettings> : FieldsContainer<TFieldSettings>
        where TFieldSettings : IFieldSettingsContainer
    {
        private int _currentPropertyId = 0;

        public override void AddOrUpdate(TFieldSettings settings)
        {
            settings.Index = _currentPropertyId++;

            base.AddOrUpdate(settings);
        }
    }

    public class FieldsContainer<TFieldSettings> : IFieldsContainer<TFieldSettings>
        where TFieldSettings : IFieldSettingsContainer
    {
        protected Dictionary<string, PropertySettingsContainer<TFieldSettings>> Fields { get; private set; }

        public FieldsContainer()
        {
            Fields = new Dictionary<string, PropertySettingsContainer<TFieldSettings>>();
        }

        public virtual void AddOrUpdate(TFieldSettings settings)
        {
            var propertySettings = new PropertySettingsContainer<TFieldSettings>
            {
                PropertySettings = settings,
                Index = settings.Index.GetValueOrDefault()
            };

            Fields[settings.UniqueKey] = propertySettings;
        }

        public virtual IEnumerable<TFieldSettings> OrderedFields
        {
            get
            {
                return Fields.Values
                    .OrderBy(settings => settings.Index)
                    .Select(x => x.PropertySettings);
            }
        }
    }

    public class PropertySettingsContainer<TPropertySettings> where TPropertySettings : IFieldSettings
    {
        public int Index { get; set; }
        public TPropertySettings PropertySettings { get; set; }
    }
}