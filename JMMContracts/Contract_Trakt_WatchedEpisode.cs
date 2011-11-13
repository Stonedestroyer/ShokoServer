﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMMContracts
{
	public class Contract_Trakt_WatchedEpisode
	{
		public int Watched { get; set; }
		public DateTime? WatchedDate { get; set; }

		public string Episode_Season { get; set; }
		public string Episode_Number { get; set; }
		public string Episode_Title { get; set; }
		public string Episode_Overview { get; set; }
		public string Episode_Url { get; set; }
		public string Episode_Screenshot { get; set; }

		public Contract_TraktTVShowResponse TraktShow { get; set; }

		public Contract_AnimeSeries AnimeSeries { get; set; }
		public Contract_AniDBAnime Anime { get; set; }
	}
}