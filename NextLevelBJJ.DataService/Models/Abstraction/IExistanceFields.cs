namespace NextLevelBJJ.DataServices.Models.Abstraction
{
    public interface IExistanceFields
    {
        bool IsEnabled { get; set; }
        bool IsDeleted { get; set; }
    }
}
