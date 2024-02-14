namespace BlApi;
/// <summary>
/// 
/// </summary>
public interface IEngineer
{
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);
    public BO.Engineer Read(int id);
    public void Create(BO.Engineer engineer);
    public void Delete(int id);
    public void Update(BO.Engineer engineer);

    public bool EngineerIsAvailabe(int id);

}
