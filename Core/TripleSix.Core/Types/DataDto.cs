namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO dữ liệu.
    /// </summary>
    public abstract class DataDto : BaseDto,
        IDataDto
    {
        private readonly HashSet<string> _propertyTracking = new ();

        /// <inheritdoc/>
        public virtual bool IsAnyPropertyChanged()
        {
            return _propertyTracking.Any();
        }

        /// <inheritdoc/>
        public virtual bool IsPropertyChanged(string name)
        {
            return _propertyTracking.Any(x => x == name);
        }

        /// <inheritdoc/>
        public virtual void SetPropertyChanged(string name, bool value)
        {
            if (value) _propertyTracking.Add(name);
            else _propertyTracking.RemoveWhere(x => x == name);
        }
    }
}
