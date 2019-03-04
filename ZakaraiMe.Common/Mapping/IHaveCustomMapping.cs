namespace ZakaraiMe.Common.Mapping
{
    using AutoMapper;
    
    /// <summary>
    /// Interfaces for viewmodels which need custom mapping logic.
    /// </summary>
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile profile);
    }
}
