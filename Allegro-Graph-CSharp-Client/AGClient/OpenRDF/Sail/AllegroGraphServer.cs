using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail
{
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

        public string Url
        {
            get
            {
                return _service.Url;
            }
        }
        public string Version
        {
            get
            {
                return _agClient.GetVersion();
            }
        }

        public string[] ListCatalogs()
        {
            return _agClient.ListCatalogs();
        }

        public Catalog OpenCatalog(string name = null)
        {
            return new Catalog(_agClient.OpenCatalog(name));
        }

        public string GetInitFile()
        {
            return _agClient.GetInitFile();  
        }

        public void SetInitFile(string content = null, bool restart = true)
        {
            _agClient.SetInitFile(content, restart);
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

    public class Catalog
    {
        private AGCatalog _agCatalog;
        public AGCatalog agCatalog
        {
            get { return _agCatalog; }
        }
        public string Url
        {
            get { return _agCatalog.Url; }
        }
        public Catalog(AGCatalog catalog)
        {
            _agCatalog = catalog;
        }
        public Repository CreateRepository(string repName)
        {
            _agCatalog.CreateRepository(repName);
            return new Repository(this, repName);
        }
        public void DeleteRepository(string repName)
        {
            _agCatalog.DeleteRepository(repName);
        }
        public string GetName()
        {
            return _agCatalog.GetName();
        }
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
