﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using PokemonApiApp;

namespace ProjectGallery.Controls;
/// <summary>
/// Interaction logic for ProjectButton.xaml
/// </summary>
public partial class ProjectButton : UserControl {
	 
	public ProjectButton(IProjectMeta project) {
		InitializeComponent();
		
		DataContext = project;

		MainButton.Click += (sender, e) => project.Run();
	}

    public Project Project { get; internal set; }
    public Action<object, RoutedEventArgs> Click { get; internal set; }

    /*private void Handle_Click(object sender, RoutedEventArgs e) {

	}*/
}
