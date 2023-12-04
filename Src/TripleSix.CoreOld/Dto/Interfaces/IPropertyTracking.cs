namespace TripleSix.CoreOld.Dto
{
    public interface IPropertyTracking
    {
        void ClearPropertyTracking();

        bool IsAnyPropertyChanged();

        bool IsPropertyChanged(string propertyName);

        void SetPropertyChanged(string propertyName, bool value);
    }
}