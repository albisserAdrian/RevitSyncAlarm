using System;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

using System.Windows.Media; // for the graphics


// use an alias because Autodesk.Revit.UI 
// uses classes which have same names:

using adWin = Autodesk.Windows;

namespace RevitSyncAlarm
{
    public class App : IExternalApplication
    {
        private BasicFileInfo info;
        private Stopwatch stopwatch;

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Register events
                application.ControlledApplication.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(OnDocumentOpened);
                application.ControlledApplication.DocumentSynchronizingWithCentral += new EventHandler<DocumentSynchronizingWithCentralEventArgs>(OnSyncCentralStart);
                application.ControlledApplication.DocumentSynchronizedWithCentral += new EventHandler<DocumentSynchronizedWithCentralEventArgs>(OnSyncCentralEnd);
                application.ControlledApplication.DocumentClosed += new EventHandler<DocumentClosedEventArgs>(OnDocumentClosed);
                application.Idling += OnIdling;
            }
            catch (Exception)
            {
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication application)
        {
            // Unregister Events
            application.ControlledApplication.DocumentOpened -= OnDocumentOpened;
            application.ControlledApplication.DocumentSynchronizingWithCentral -= OnSyncCentralStart;
            application.ControlledApplication.DocumentSynchronizedWithCentral -= OnSyncCentralEnd;
            application.ControlledApplication.DocumentClosed -= OnDocumentClosed;
            application.Idling -= OnIdling;
            return Result.Succeeded;
        }

        public void OnDocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            info = BasicFileInfo.Extract(e.Document.PathName);
            TaskDialog.Show("Test", info.IsWorkshared.ToString());
            if (info.IsWorkshared)
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
            }
        }
        public void OnSyncCentralStart(object sender, DocumentSynchronizingWithCentralEventArgs e)
        {
            stopwatch.Reset();
        }

        public void OnSyncCentralEnd(object sender, DocumentSynchronizedWithCentralEventArgs e)
        {
            stopwatch.Start();
            adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;
            foreach (adWin.RibbonTab tab in ribbon.Tabs)
            {
                foreach (adWin.RibbonPanel panel in tab.Panels)
                {

                    panel.CustomPanelBackground = (SolidColorBrush)new BrushConverter().ConvertFromString("#F6F6F6");
                    panel.CustomPanelTitleBarBackground = (SolidColorBrush)new BrushConverter().ConvertFromString("#F6F6F6");
                }
            }
        }

        public void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (info.IsWorkshared)
            {
                stopwatch.Stop();
                adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;
                foreach (adWin.RibbonTab tab in ribbon.Tabs)
                {
                    foreach (adWin.RibbonPanel panel in tab.Panels)
                    {
                        panel.CustomPanelBackground = (SolidColorBrush)new BrushConverter().ConvertFromString("#F6F6F6");
                        panel.CustomPanelTitleBarBackground = (SolidColorBrush)new BrushConverter().ConvertFromString("#F6F6F6");
                    }
                }
            }
        }

        public void OnIdling(object sender, IdlingEventArgs e)
        {
            if (info.IsWorkshared)
            {
                int Twenty = 1200000;
                int Forty = 2400000;
                //int Twenty = 30000;
                //int Forty = 60000;
                adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;
                if (stopwatch.ElapsedMilliseconds > Twenty && stopwatch.ElapsedMilliseconds < Forty)
                {
                    foreach (adWin.RibbonTab tab in ribbon.Tabs)
                    {
                        foreach (adWin.RibbonPanel panel in tab.Panels)
                        {
                            panel.CustomPanelBackground = new SolidColorBrush(Colors.Yellow);
                            panel.CustomPanelTitleBarBackground = new SolidColorBrush(Colors.Yellow);
                        }
                    }
                }
                if (stopwatch.ElapsedMilliseconds > Forty)
                {
                    foreach (adWin.RibbonTab tab in ribbon.Tabs)
                    {
                        foreach (adWin.RibbonPanel panel in tab.Panels)
                        {
                            panel.CustomPanelBackground = new SolidColorBrush(Colors.Red);
                            panel.CustomPanelTitleBarBackground = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }
        void ComponentManager_UIElementActivated(object sender, Autodesk.Windows.UIElementActivatedEventArgs e)
        {

        }

    }
}
