namespace Poseidon.Config
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string ConnectionStringDocker { get; set; } // Docker connection string
        public string ConnectionStringK8s { get; set; } // Kubernetes connection string
        public string DatabaseName { get; set; }
    }
}
