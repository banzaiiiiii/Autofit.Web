using AutoFit.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.Services
{
	public class HomeService : BaseService
	{
		public async Task<HomeIndexViewModel> LoadAllBuisnesses() // sollte eine Liste von buissness laden, lädt aber nur das eine hier
		{
			var buissnessView = new HomeIndexViewModel()
			                    {
				                    FirmenName = "AutoFit",
				                    Ort = "Rochlitz"
			                    };
			return buissnessView;
		} 
	
		

		

	}
}
