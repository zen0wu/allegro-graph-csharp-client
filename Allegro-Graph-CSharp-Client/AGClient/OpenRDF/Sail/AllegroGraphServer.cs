using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail
{
    /// <summary>
    /// Connects to an AllegroGraph HTTP Server
    /// </summary>
    public class AllegroGraphServer
    {
        private AGClient.Mini.AGServerInfo _service;
        private AGClient.Mini.AGClient _agClient;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">server</param>
        /// <param name="port">port number，default 10035</param>
        /// <param name="user">user name,default null</param>
        /// <param name="password">password，default null</param>
        /// <returns></returns>
        public AllegroGraphServer(string host, int port = 10035, string user = null, string password = null)
        {
            _service = new AGServerInfo(string.Format("http://{0}:{1}", host, port), user, password);
            _agClient = new Mini.AGClient(_service);
        }

        /// <summary>
        /// Return the server's URL.
        /// </summary>
        public string Url { get { return _service.Url; } }

        /// <summary>
        /// Returns the version of the AllegroGraph server, as a string.
        /// </summary>
        public string Version { get { return _agClient.GetVersion(); } }

        /// <summary>
        /// Return the date on which the server was built.
        /// </summary>
        public DateTime Date { get { return _agClient.GetBuiltDate(); } }


        /// <summary>
        /// Cause the server to re-read its configuration file, 
        /// and update itself to reflect the new configuration
        /// </summary>
        public void ServerReConfigure()
        {
            _agClient.ReConfigure();
        }

        /// <summary>
        /// re-open server log file
        /// </summary>
        public void ReopenLog()
        {
            _agClient.ReopenLog();
        }

        /// <summary>
        /// Returns string set containing the names of the server's catalogs. 
        /// </summary>
        /// <returns> a set of catalogs name</returns>
        public string[] ListCatalogs()
        {
            return _agClient.ListCatalogs();
        }

        /// <summary>
        /// Create catalog,if exist return null else return new catalog
        /// </summary>
        /// <param name="Name">catalog name</param>
        /// <returns>Catalog</returns>
        public Catalog CreateCatalog(string name)
        {
            AGCatalog agCatalog = _agClient.CreateCatalog(name);
            if (agCatalog == null)
                return null;
            else
                return new Catalog(agCatalog);
        }

        /// <summary>
        /// Open a catalog
        /// </summary>
        /// <param name="name">catalog name</param>
        /// <returns>Catalog</returns>
        public Catalog OpenCatalog(string name = null)
        {
            return new Catalog(_agClient.OpenCatalog(name));
        }

        /// <summary>
        /// Delete catalog
        /// </summary>
        /// <param name="name">catalog name</param>
        public void DeleteCatalog(string name)
        {
            _agClient.DeleteCatalog(name);
        }

        /// <summary>
        /// Retrieve the contents of the server initialization file,only for superuser.
        /// </summary>
        /// <returns></returns>
        public string GetInitFile()
        {
            return _agClient.GetInitFile();
        }

        /// <summary>
        ///  Replace the current initialization file contents with the
        /// 'content' string or remove if null. 
        /// </summary>
        /// <param name="content">init file content</param>
        /// <param name="restart">
        ///     defaults to true, specifies whether any running shared back-ends should
        ///     be shut down, so that subsequent requests will be handled by
        ///     back-ends that include the new code.
        /// </param>
        public void SetInitFile(string content = null, bool restart = true)
        {
            _agClient.SetInitFile(content, restart);
        }

        /// <summary>
        /// Remove the server's initialization file
        /// </summary>
        public void DeleteInitFile()
        {
            _agClient.DeleteInitFile();
        }

        public Repository OpenSession(string spec, bool autoCommit = false, int lifetime = -1, bool loadInitFile = false)
        {
            AGRepository _agRepository = _agClient.OpenSession(spec, autoCommit, lifetime, loadInitFile);
            return new Repository(_agRepository);
        }

        public Repository OpenFederated(string[] specs, bool autoCommit = false, int lifetime = -1, bool loadInitFile = false)
        {
            string spec = Spec.Federate(specs);
            return OpenSession(spec, autoCommit, lifetime, loadInitFile);
        }
    }

    /// <summary>
    /// Container of multiple repositories (triple stores).
    /// </summary>
    public class Catalog
    {
        private AGCatalog _agCatalog;
        public AGCatalog agCatalog { get { return _agCatalog; } }
        public string Url { get { return _agCatalog.Url; } }
        public Catalog(AGCatalog catalog) { _agCatalog = catalog; }

        /// <summary>
        /// Get the protocol version of the Sesame interface
        /// </summary>
        /// <returns>protocol version</returns>
        public string GetSesameProtocolVersion()
        {
            return AGRequestService.DoReqAndGet<string>(agCatalog, "GET", "/protocol ");
        }

        /// <summary>
        /// Creates a new Repository within the Catalog. 
        /// </summary>
        /// <param name="repName">string identifying the repository</param>
        /// <returns></returns>
        public Repository CreateRepository(string repName)
        {
            _agCatalog.CreateRepository(repName);
            return new Repository(this, repName);
        }

        /// <summary>
        /// Deletes the named Respository from the Catalog
        /// </summary>
        /// <param name="repName">Repository name</param>
        public void DeleteRepository(string repName)
        {
            _agCatalog.DeleteRepository(repName);
        }

        /// <summary>
        /// Returns a string containing the name of this Catalog
        /// </summary>
        /// <returns>Catalog name</returns>
        public string GetName()
        {
            return _agCatalog.GetName();
        }

        /// <summary>
        /// Returns string set of repository names (triple stores) managed by this Catalog.
        /// </summary>
        /// <returns>repository names</returns>
        public string[] ListRepositories()
        {
            return _agCatalog.ListRepositories();
        }

        /// <summary>
        /// get repository
        /// </summary>
        /// <param name="name">repository name</param>
        /// <param name="AccessVerb">access method</param>
        /// <returns>Repository</returns>
        public Repository GetRepository(string name, AccessVerb access_verb = AccessVerb.OPEN)
        {
            string[] repositories = this.ListRepositories();
            bool exist = repositories.Contains(name);
            if (access_verb == AccessVerb.ACCESS)
            {
                if (!exist)
                {
                    return this.CreateRepository(name);
                }
            }
            else if (access_verb == AccessVerb.RENEW)
            {
                if (exist)
                    this.DeleteRepository(name);
                return this.CreateRepository(name);
            }
            else if (access_verb == AccessVerb.CREATE)
            {
                if (exist)
                {
                    throw new Allegro_Graph_CSharp_Client.AGClient.Util.AGRequestException(string.Format("Can't create triple store named {0} because a store with that name already exists.", name));
                }
                return this.CreateRepository(name);
            }
            else if (access_verb == AccessVerb.OPEN)
            {
                if (!exist)
                {
                    throw new Allegro_Graph_CSharp_Client.AGClient.Util.AGRequestException(string.Format("Can't open a triple store named {0} because there is none.", name));
                }
            }
            return new Repository(this, name);
        }
    }
}
