namespace FengWo.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly FengWoDbContext _context;

        public InitialHostDbBuilder(FengWoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultMenusCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
