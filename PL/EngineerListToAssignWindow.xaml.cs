﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for EngineerListToAssignWindow.xaml
/// </summary>
public partial class EngineerListToAssignWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListToAssignWindow), new PropertyMetadata(null));

    BO.Task task = new BO.Task();

    public EngineerListToAssignWindow(ref BO.Task t)
    {
        task = t;
        EngineerList = s_bl.Engineer.ReadAll(engineer => !(engineer.Level < task.Copmlexity));
        InitializeComponent();
    }

    /// <summary>
    /// EngineerList selected event
    /// </summary>
    private void EngineerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var engineer = (sender as ListView)!.SelectedItem as BO.Engineer;
        //s_bl.AssignEngineer(engineer!.Id, task.Id);
        if (engineer != null)
        {
            task.Engineer = new BO.EngineerInTask() { Id = engineer.Id, Name = engineer.Name };
            Close();
        }
        
    }
}