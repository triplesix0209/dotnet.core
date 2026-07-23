using TripleSix.Core.Attributes;

namespace Sample.Domain
{
    public enum Settings
    {
        [Setting(DefaultValue = null)]
        Sample,
    }
}
