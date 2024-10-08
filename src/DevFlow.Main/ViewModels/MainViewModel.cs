﻿using System.Linq;
using System.Collections.ObjectModel;
using DevFlow.Data;
using DevFlow.Data.Menu;
using DevFlow.Data.Theme;
using DevFlow.Data.Works;
using DevFlow.Languages.ViewModels;
using DevFlow.Languages.Views;
using DevFlow.Main.Views;
using DevFlow.Menus.ViewModels;
using DevFlow.Skins.ViewModels;
using DevFlow.Skins.Views;
using DevFlow.Foundation.Flowbase;
using DevFlow.Foundation.Flowcore;
using DevFlow.Foundation.Mvvm;
using DevFlow.Data.Language;
using DevFlow.Colors.Views;
using DevFlow.Colors.ViewModels;
using System;
using DevFlow.Finders.Local.ViewModel;
using DevFlow.Finders.UI.Views;

namespace DevFlow.Main.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		#region Variables

		private readonly FlowTheme theme;
		private readonly FlowCulture culture;
		#endregion

		#region Wallpaper

		public string Wallpaper { get; set; }
		#endregion

		#region Menu

		public QuickSlotViewModel Menu { get; }
		#endregion

		#region Theme

		public SwitchSkinViewModel Skin { get; }
		#endregion

		#region Translate

		private TranslatorViewModel Translate { get; }
		#endregion

		#region Works

		public ObservableCollection<WorkspaceModel> Works { get; set; }
		#endregion

		#region Constructor

		public MainViewModel()
		{
			Wallpaper = "/DevFlow.Resources;component/Images/wallpaper-08.jpg";

			Works = new ObservableCollection<WorkspaceModel>();
			Menu = new QuickSlotViewModel(new RelayCommand<MenuModel>(MenuSelected));
		}

		public MainViewModel(FlowTheme _theme, FlowCulture _culture) : this()
		{
			theme = _theme;
			culture = _culture;
			Skin = new SwitchSkinViewModel(SkinSelected, theme.GetCurrentTheme());
			Translate = new TranslatorViewModel(LanguageChanged, culture.GetCurrentLanguage());
		}
		#endregion

		#region MenuSelected

		private void MenuSelected(MenuModel menu)
		{
			if (Works.FirstOrDefault(x => x.Menu.Equals(menu)) is null)
			{
				IFlowElement content = null;

				switch (menu.IconType)
				{
					case GeoIcon.FolderOpenOutline: content = new Finder().UseViewModel(new FinderViewModel()); break;
					case GeoIcon.EyedropperVariant: content = new ColorSpoid().UseViewModel(new ColorSpoidViewModel()); break;
					case GeoIcon.Palette: content = new SwitchSkin().UseViewModel(Skin); break;
					case GeoIcon.Web: content = new Translator().UseViewModel(Translate); break;
					case GeoIcon.Close: Environment.Exit(0); break;
					default: content = new EmptyView(); break;
				}
				content.OnShow(menu);
			}

		}
		#endregion

		#region SkinSelected

		private void SkinSelected(SkinModel theme)
		{
			this.theme.Switch(theme.Skin);
			FlowConfig.SaveTheme(theme.Skin);
		}
		#endregion

		#region LanguageChanged

		private void LanguageChanged(LanguageModel culture)
		{
			this.culture.Switch(culture.Language);
			FlowConfig.SaveLanguage(culture.Language);
		}
		#endregion
	}
}
