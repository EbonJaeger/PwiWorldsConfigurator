using System;
using System.IO;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel) => Build();

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnOpen(object sender, EventArgs e)
    {
        // Reset the window back to original size
        this.GetDefaultSize(out int width, out int height);
        this.Resize(width, height);

        // Create and display a fileChooserDialog
        var chooser = new FileChooserDialog(
           "Please select a worlds.json file to edit...",
           this,
           FileChooserAction.Open,
           "Cancel", ResponseType.Cancel,
           "Open", ResponseType.Accept);

        if (chooser.Run() == (int) ResponseType.Accept)
        {
            // Check the file name before bothering to open it
            if (!chooser.Filename.EndsWith("worlds.json", StringComparison.Ordinal))
            {
                var dialog = new MessageDialog(this,
                                               DialogFlags.DestroyWithParent,
                                               MessageType.Warning,
                                               ButtonsType.Close,
                                               "Not a valid configuration file.");
                dialog.Run();
                dialog.Destroy();
                chooser.Destroy();
                return;
            }

            // Open the file
            using(var file = File.OpenText(chooser.Filename))
            {
                // Shove the text into the text view
                worldsJsonView.Buffer.Text = file.ReadToEnd();

                this.Title = "PWI World Configurator -- " + chooser.Filename;
            }
        }

        chooser.Destroy();
    }

    protected void OnQuit(object sender, EventArgs e)
    {
        Application.Quit();
    }

    protected void OnSave(object sender, EventArgs e)
    {
    }

    protected void OnAbout(object sender, EventArgs e)
    {
    }
}
