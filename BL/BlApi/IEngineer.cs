namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// returns a collection of all BO.Engineers that exist and satisfy a condition if given
    /// </summary>
    /// <param name="filter">a boolean function that determines for each engineer if it satisfies the condition or not</param>
    /// <returns>an IEnumerable of BO engineers</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// returns a BO.Engineer according to the Id
    /// </summary>
    /// <param name="id">the Id of the engineer that will be returned</param>
    /// <returns>a complete BO.Engineer</returns>
    public BO.Engineer Read(int id);

    /// <summary>
    /// adds a new engineer to the data layer
    /// </summary>
    /// <param name="engineer">the new engineer that will be added</param>
    public void Create(BO.Engineer engineer);

    /// <summary>
    /// deletes an engineer from the data layer
    /// </summary>
    /// <param name="id">the id of the engineer that you want to delete</param>
    public void Delete(int id);

    /// <summary>
    /// Updates the data of an engineer in the data layer to the data of the engineer received
    /// </summary>
    /// <param name="engineer">the engineer to be updated with the new data</param>
    public void Update(BO.Engineer engineer);

    /// <summary>
    /// help function that checks whether an engineer is available (not in the middle of doing a task)
    /// </summary>
    /// <param name="id">id of the checked engineer</param>
    /// <returns>true if the engineer is available and false if he does not</returns>
    public bool EngineerIsAvailabe(int id);

}
