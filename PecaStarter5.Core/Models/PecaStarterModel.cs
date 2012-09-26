﻿using System;
using System.Collections.Generic;
using Progressive.PecaStarter5.Models.Daos;
using Progressive.PecaStarter5.Models.ExternalYellowPages;
using Progressive.PecaStarter5.Models.Services;
using Progressive.PecaStarter5.Models.YellowPagesXml;
using Progressive.Peercast4Net;
using Progressive.Peercast4Net.Datas;
using Progressive.PecaStarter5.Plugin;
using Progressive.PecaStarter5.Models.Broadcasts;

namespace Progressive.PecaStarter5.Models
{
    // TODO: VMにあるロジック・エンティティを可能な限りここに移動
    public class PecaStarterModel
    {
        private readonly Peercast m_peercast;
        private readonly PeercastStation m_peercastStation;
        private readonly IExternalResource m_externalResource;
        private readonly List<IExternalYellowPages> m_externalYellowPagesList;

        public PecaStarterModel(string title, IExternalResource externalResource)
        {
            Title = title;
            m_externalResource = externalResource;
            m_peercast = new Peercast();
            m_peercastStation = new PeercastStation();
            Plugins = externalResource.GetPlugins();
            var tuple = GetYellowPagesLists();
            m_externalYellowPagesList = tuple.Item2;
            YellowPagesList = tuple.Item1;

            BroadcastModel = new BroadcastModel(Plugins);
            Configuration = new ConfigurationDao(externalResource).Get();
            Configuration.DefaultLogPath = externalResource.DefaultLogPath;
            Service = new PeercastService(m_externalYellowPagesList, Plugins, Configuration);
        }

        public BroadcastModel BroadcastModel { get; private set; }
        public string Title { get; private set; }
        public PeercastService Service { get; private set; }
        public Configuration Configuration { get; private set; }
        public IEnumerable<IPlugin> Plugins { get; private set; }
        public List<IYellowPages> YellowPagesList { get; private set; }

        public void Save()
        {
            new ConfigurationDao(m_externalResource).Put(Configuration);
        }

        private Tuple<List<IYellowPages>, List<IExternalYellowPages>> GetYellowPagesLists()
        {
            var yellowPagesList = new List<IYellowPages>();
            var externalYellowPagesList = new List<IExternalYellowPages>();
            foreach (var xml in m_externalResource.GetYellowPagesDefineInputStream())
            {
                var yp = YellowPagesParserFactory.GetInstance(xml).GetInstance();
                yellowPagesList.Add(yp);
                if (yp.IsExternal)
                {
                    externalYellowPagesList.Add((IExternalYellowPages)yp);
                }
            }
            return Tuple.Create(yellowPagesList, externalYellowPagesList);
        }
    }
}
