namespace VirtualSports.BLL.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionStorage
    {
        public void Add(string token);

        public bool Contains(string token);
    }
}