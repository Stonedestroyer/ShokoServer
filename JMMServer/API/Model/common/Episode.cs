﻿using JMMContracts.PlexAndKodi;
using JMMServer.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace JMMServer.API.Model.common
{
    [DataContract]
    public class Episode : BaseDirectory
    {
        public override string type { get { return "ep"; } }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string season { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string votes { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int view { get; set; }
        [DataMember]
        public string eptype { get; set; }
        [DataMember]
        public int epnumber { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public List<RawFile> files { get; set; }

        public Episode()
        {

        }

        internal static Episode GenerateFromAnimeEpisodeID(int anime_episode_id, int uid, int level)
        {
            Episode ep = new Episode();

            if (anime_episode_id > 0)
            {
                ep = GenerateFromAnimeEpisode(Repositories.RepoFactory.AnimeEpisode.GetByID(anime_episode_id), uid, level);
            }

            return ep;
        }

        internal static Episode GenerateFromAnimeEpisode(AnimeEpisode aep, int uid, int level)
        {
            Episode ep = new Episode();
            if (aep != null)
            {
                JMMContracts.Contract_AnimeEpisode cae = aep.GetUserContract(uid);
                if (cae != null)
                {

                    ep.id = aep.AnimeEpisodeID;
                    ep.art = new ArtCollection();
                    ep.name = aep.PlexContract?.Title;
                    ep.summary = aep.PlexContract?.Summary;
                    ep.year = aep.PlexContract?.Year;
                    ep.air = aep.PlexContract?.AirDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ep.votes = cae.AniDB_Votes;
                    ep.rating = aep.PlexContract?.Rating;
                    ep.userrating = aep.PlexContract?.UserRating;
                    double rating;
                    if (double.TryParse(ep.rating, out rating))
                    {
                        // 0.1 should be the absolute lowest rating
                        if (rating > 10) ep.rating = (rating / 100).ToString().Replace(',','.');
                    }

                    ep.view = cae.IsWatched;
                    ep.epnumber = cae.EpisodeNumber;
                    ep.eptype = aep.EpisodeTypeEnum.ToString();

                    ep.season = aep.PlexContract?.Season;

                    // until fanart refactor this will be good for start
                    if (aep.PlexContract?.Thumb != null) { ep.art.thumb.Add(new Art() { url = APIHelper.ConstructImageLinkFromRest(aep.PlexContract?.Thumb), index = 0 }); }
                    if (aep.PlexContract?.Art != null) { ep.art.fanart.Add(new Art() { url = APIHelper.ConstructImageLinkFromRest(aep.PlexContract?.Art), index = 0 }); }

                    if (level > 0)
                    {
                        List<VideoLocal> vls = aep.GetVideoLocals();
                        if (vls.Count > 0)
                        {
                            ep.files = new List<RawFile>();
                            foreach (VideoLocal vl in vls)
                            {
                                ep.files.Add(new RawFile(vl, (level-1), uid));
                            }
                        }
                    }
                }
            }

            return ep;
        }
    }
}
