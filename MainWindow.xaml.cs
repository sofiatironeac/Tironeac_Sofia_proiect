using AirlineModel;
using System;
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
using System.Data.Entity;
using System.Data;
using static Tironeac_Sofia_proiect.Validation;

namespace Tironeac_Sofia_proiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AirlineEntitiesModel ctx = new AirlineEntitiesModel();
        CollectionViewSource clientViewSource;
        CollectionViewSource flightViewSource;
        CollectionViewSource clientBookedFlightsViewSource;

        Binding txtFirstNameBinding = new Binding();
        Binding txtLastNameBinding = new Binding();
        Binding txtEmailBinding = new Binding();
        Binding txtNationalityBinding = new Binding();

        Binding txtFromBinding = new Binding();
        Binding txtToBinding = new Binding();
        //Binding txtDateBinding = new Binding();
        Binding txtDurationBinding = new Binding();
        Binding txtPriceBinding = new Binding();

        Binding cmbClientsBinding = new Binding();
        Binding cmbFlightsBinding = new Binding();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            txtFirstNameBinding.Path = new PropertyPath("FirstName");
            txtLastNameBinding.Path = new PropertyPath("LastName");
            txtEmailBinding.Path = new PropertyPath("Email");
            txtNationalityBinding.Path = new PropertyPath("Nationality");

            firstNameTextBox.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, txtLastNameBinding);
            emailTextBox.SetBinding(TextBox.TextProperty, txtEmailBinding);
            nationalityTextBox.SetBinding(TextBox.TextProperty, txtNationalityBinding);

            txtFromBinding.Path = new PropertyPath("From");
            txtToBinding.Path = new PropertyPath("To");
            //txtDateBinding.Path = new PropertyPath("Date");
            txtDurationBinding.Path = new PropertyPath("Duration");
            txtPriceBinding.Path = new PropertyPath("Price");

            fromTextBox.SetBinding(TextBox.TextProperty, txtFromBinding);
            toTextBox.SetBinding(TextBox.TextProperty, txtToBinding);
           // dateDatePicker.SetBinding(DatePicker.DisplayDateProperty, txtDateBinding);
            durationTextBox.SetBinding(TextBox.TextProperty, txtDurationBinding);
            priceTextBox.SetBinding(TextBox.TextProperty, txtPriceBinding);

            cmbFlightsBinding.Path = new PropertyPath("From");
            cmbClientsBinding.Path = new PropertyPath("LastName");

        }

        private void BindDataGrid()
        {
            var queryBookedFlights = from bookedflight in ctx.BookedFlights
                             join client in ctx.Clients on bookedflight.ClientId equals
                             client.Id
                             join flight in ctx.Flights on bookedflight.FlightId
                 equals flight.Id
                             select new
                             {
                                 bookedflight.Id,
                                 bookedflight.FlightId,
                                 bookedflight.ClientId,
                                 client.FirstName,
                                 client.LastName,
                                 flight.From,
                                 flight.To
                             };
            clientBookedFlightsViewSource.Source = queryBookedFlights.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clientViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientViewSource")));
            clientViewSource.Source = ctx.Clients.Local;
            ctx.Clients.Load();

            flightViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("flightViewSource")));
            flightViewSource.Source = ctx.Flights.Local;
            ctx.Flights.Load();

            clientBookedFlightsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientBookedFlightsViewSource")));
            //clientBookedFlightsViewSource.Source = ctx.BookedFlights.Local;

            cmbClients.ItemsSource = ctx.Clients.Local;
            //cmbClients.DisplayMemberPath = "LastName";
            cmbClients.SelectedValuePath = "Id";

            cmbFlights.ItemsSource = ctx.Flights.Local;
            //cmbFlights.DisplayMemberPath = "From";
            cmbFlights.SelectedValuePath = "Id";


            BindDataGrid();
        }

        private void btnSaveClient_Click(object sender, RoutedEventArgs e)
        {
            Client client = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    client = new Client()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                        Email = emailTextBox.Text.Trim(),
                        Nationality = nationalityTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Clients.Add(client);
                    clientViewSource.View.Refresh();
                    SetValidationBinding();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNewClient.IsEnabled = true;
                btnEditClient.IsEnabled = true;
                btnDeleteClient.IsEnabled = true;

                btnSaveClient.IsEnabled = false;
                btnCancelClient.IsEnabled = false;

                clientDataGrid.IsEnabled = true;

                btnPrevClient.IsEnabled = true;
                btnNextClient.IsEnabled = true;

                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
                emailTextBox.IsEnabled = false;
                nationalityTextBox.IsEnabled = false;

            }
            else
            {
                if (action == ActionState.Edit)
                {
                    try
                    {
                        client = (Client)clientDataGrid.SelectedItem;
                        client.FirstName = firstNameTextBox.Text.Trim();
                        client.LastName = lastNameTextBox.Text.Trim();
                        client.Email = emailTextBox.Text.Trim();
                        client.Nationality = nationalityTextBox.Text.Trim();
                        //salvam modificarile
                        SetValidationBinding();
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    btnNewClient.IsEnabled = true;
                    btnEditClient.IsEnabled = true;
                    btnDeleteClient.IsEnabled = true;

                    btnSaveClient.IsEnabled = false;
                    btnCancelClient.IsEnabled = false;

                    clientDataGrid.IsEnabled = true;

                    btnPrevClient.IsEnabled = true;
                    btnNextClient.IsEnabled = true;

                    firstNameTextBox.IsEnabled = false;
                    lastNameTextBox.IsEnabled = false;
                    emailTextBox.IsEnabled = false;
                    nationalityTextBox.IsEnabled = false;

                    firstNameTextBox.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
                    lastNameTextBox.SetBinding(TextBox.TextProperty, txtLastNameBinding);
                    emailTextBox.SetBinding(TextBox.TextProperty, txtEmailBinding);
                    nationalityTextBox.SetBinding(TextBox.TextProperty, txtNationalityBinding);

                    SetValidationBinding();

                    clientViewSource.View.Refresh();
                    // pozitionarea pe item-ul curent
                    clientViewSource.View.MoveCurrentTo(client);
                }
                else if (action == ActionState.Delete)
                {
                    try
                    {
                        client = (Client)clientDataGrid.SelectedItem;
                        ctx.Clients.Remove(client);
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    btnNewClient.IsEnabled = true;
                    btnEditClient.IsEnabled = true;
                    btnDeleteClient.IsEnabled = true;

                    btnSaveClient.IsEnabled = false;
                    btnCancelClient.IsEnabled = false;

                    clientDataGrid.IsEnabled = true;

                    btnPrevClient.IsEnabled = true;
                    btnNextClient.IsEnabled = true;

                    firstNameTextBox.IsEnabled = false;
                    lastNameTextBox.IsEnabled = false;
                    emailTextBox.IsEnabled = false;
                    nationalityTextBox.IsEnabled = false;

                    firstNameTextBox.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
                    lastNameTextBox.SetBinding(TextBox.TextProperty, txtLastNameBinding);
                    emailTextBox.SetBinding(TextBox.TextProperty, txtEmailBinding);
                    nationalityTextBox.SetBinding(TextBox.TextProperty, txtNationalityBinding);

                    SetValidationBinding();

                    clientViewSource.View.Refresh();
                }
            }
        }

        private void btnNextClient_Click(object sender, RoutedEventArgs e)
        {
            clientViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevClient_Click(object sender, RoutedEventArgs e)
        {
            clientViewSource.View.MoveCurrentToPrevious();
        }
        private void btnNewClient_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewClient.IsEnabled = false;
            btnEditClient.IsEnabled = false;
            btnDeleteClient.IsEnabled = false;

            btnSaveClient.IsEnabled = true;
            btnCancelClient.IsEnabled = true;

            clientDataGrid.IsEnabled = false;

            btnPrevClient.IsEnabled = false;
            btnNextClient.IsEnabled = false;

            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            emailTextBox.IsEnabled = true;
            nationalityTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(emailTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(nationalityTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            nationalityTextBox.Text = "";
            Keyboard.Focus(firstNameTextBox);
        }

        private void btnEditClient_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();
            string tempEmail = emailTextBox.Text.ToString();
            string tempNationality = nationalityTextBox.Text.ToString();

            btnNewClient.IsEnabled = false;
            btnEditClient.IsEnabled = false;
            btnDeleteClient.IsEnabled = false;

            btnSaveClient.IsEnabled = true;
            btnCancelClient.IsEnabled = true;

            clientDataGrid.IsEnabled = false;

            btnPrevClient.IsEnabled = false;
            btnNextClient.IsEnabled = false;

            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            emailTextBox.IsEnabled = true;
            nationalityTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(emailTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(nationalityTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            emailTextBox.Text = tempEmail;
            nationalityTextBox.Text = tempNationality;
            Keyboard.Focus(firstNameTextBox);

            SetValidationBinding();
        }

        private void btnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();
            string tempEmail = emailTextBox.Text.ToString();
            string tempNationality = nationalityTextBox.Text.ToString();

            btnNewClient.IsEnabled = false;
            btnEditClient.IsEnabled = false;
            btnDeleteClient.IsEnabled = false;

            btnSaveClient.IsEnabled = true;
            btnCancelClient.IsEnabled = true;

            clientDataGrid.IsEnabled = false;

            btnPrevClient.IsEnabled = false;
            btnNextClient.IsEnabled = false;

            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;
            emailTextBox.IsEnabled = false;
            nationalityTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(emailTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(nationalityTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            emailTextBox.Text = tempEmail;
            nationalityTextBox.Text = tempNationality;

        }

        private void btnCancelClient_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNewClient.IsEnabled = true;
            btnEditClient.IsEnabled = true;
            btnDeleteClient.IsEnabled = true;

            btnSaveClient.IsEnabled = false;
            btnCancelClient.IsEnabled = false;

            clientDataGrid.IsEnabled = true;

            btnPrevClient.IsEnabled = true;
            btnNextClient.IsEnabled = true;

            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;
            emailTextBox.IsEnabled = false;
            nationalityTextBox.IsEnabled = false;

            firstNameTextBox.SetBinding(TextBox.TextProperty, txtFirstNameBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, txtLastNameBinding);
            emailTextBox.SetBinding(TextBox.TextProperty, txtEmailBinding);
            nationalityTextBox.SetBinding(TextBox.TextProperty, txtNationalityBinding);
        }

        private void btnSaveFlight_Click(object sender, RoutedEventArgs e)
        {
            Flight flight = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Flight entity
                    flight = new Flight()
                    {
                        From = fromTextBox.Text.Trim(),
                        To = toTextBox.Text.Trim(),
                        //Date = dateDatePicker.(),
                        Duration = durationTextBox.Text.Trim(),
                        Price = priceTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Flights.Add(flight);
                    flightViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNewFlight.IsEnabled = true;
                btnEditFlight.IsEnabled = true;
                btnDeleteFlight.IsEnabled = true;

                btnSaveFlight.IsEnabled = false;
                btnCancelFlight.IsEnabled = false;

                flightDataGrid.IsEnabled = true;

                btnPrevFlight.IsEnabled = true;
                btnNextFlight.IsEnabled = true;

                fromTextBox.IsEnabled = false;
                toTextBox.IsEnabled = false;
                //dateTextBox.IsEnabled = false;
                durationTextBox.IsEnabled = false;
                priceTextBox.IsEnabled = false;

            }
            else
            {
                if (action == ActionState.Edit)
                {
                    try
                    {
                        flight = (Flight)flightDataGrid.SelectedItem;
                        flight.From = fromTextBox.Text.Trim();
                        flight.To = toTextBox.Text.Trim();
                        //flight.date
                        flight.Duration = durationTextBox.Text.Trim();
                        flight.Price = priceTextBox.Text.Trim();
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    btnNewFlight.IsEnabled = true;
                    btnEditFlight.IsEnabled = true;
                    btnDeleteFlight.IsEnabled = true;

                    btnSaveFlight.IsEnabled = false;
                    btnCancelFlight.IsEnabled = false;

                    flightDataGrid.IsEnabled = true;

                    btnPrevFlight.IsEnabled = true;
                    btnNextFlight.IsEnabled = true;

                    fromTextBox.IsEnabled = false;
                    toTextBox.IsEnabled = false;
                    //dateTextBox.IsEnabled = false;
                    durationTextBox.IsEnabled = false;
                    priceTextBox.IsEnabled = false;

                    fromTextBox.SetBinding(TextBox.TextProperty, txtFromBinding);
                    toTextBox.SetBinding(TextBox.TextProperty, txtToBinding);
                    //date
                    durationTextBox.SetBinding(TextBox.TextProperty, txtDurationBinding);
                    priceTextBox.SetBinding(TextBox.TextProperty, txtPriceBinding);

                    //SetValidationBinding();

                    flightViewSource.View.Refresh();
                    // pozitionarea pe item-ul curent
                    flightViewSource.View.MoveCurrentTo(flight);
                }
                else if (action == ActionState.Delete)
                {
                    try
                    {
                        flight = (Flight)flightDataGrid.SelectedItem;
                        ctx.Flights.Remove(flight);
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    btnNewFlight.IsEnabled = true;
                    btnEditFlight.IsEnabled = true;
                    btnDeleteFlight.IsEnabled = true;

                    btnSaveFlight.IsEnabled = false;
                    btnCancelFlight.IsEnabled = false;

                    flightDataGrid.IsEnabled = true;

                    btnPrevFlight.IsEnabled = true;
                    btnNextFlight.IsEnabled = true;

                    fromTextBox.IsEnabled = false;
                    toTextBox.IsEnabled = false;
                    //dateTextBox.IsEnabled = false;
                    durationTextBox.IsEnabled = false;
                    priceTextBox.IsEnabled = false;

                    fromTextBox.SetBinding(TextBox.TextProperty, txtFromBinding);
                    toTextBox.SetBinding(TextBox.TextProperty, txtToBinding);
                    //date
                    durationTextBox.SetBinding(TextBox.TextProperty, txtDurationBinding);
                    priceTextBox.SetBinding(TextBox.TextProperty, txtPriceBinding);

                    //SetValidationBinding();

                    flightViewSource.View.Refresh();
                }
            }
        }

        private void btnNextFlight_Click(object sender, RoutedEventArgs e)
        {
            flightViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevFlight_Click(object sender, RoutedEventArgs e)
        {
            flightViewSource.View.MoveCurrentToPrevious();
        }
        private void btnNewFlight_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewFlight.IsEnabled = false;
            btnEditFlight.IsEnabled = false;
            btnDeleteFlight.IsEnabled = false;

            btnSaveFlight.IsEnabled = true;
            btnCancelFlight.IsEnabled = true;

            flightDataGrid.IsEnabled = false;

            btnPrevFlight.IsEnabled = false;
            btnNextFlight.IsEnabled = false;

            fromTextBox.IsEnabled = true;
            toTextBox.IsEnabled = true;
            //date
            durationTextBox.IsEnabled = true;
            priceTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(fromTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(toTextBox, TextBox.TextProperty);
            //date
            BindingOperations.ClearBinding(durationTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(priceTextBox, TextBox.TextProperty);
            fromTextBox.Text = "";
            toTextBox.Text = "";
            //
            durationTextBox.Text = "";
            priceTextBox.Text = "";
            Keyboard.Focus(fromTextBox);
        }

        private void btnEditFlight_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFrom = fromTextBox.Text.ToString();
            string tempTo = toTextBox.Text.ToString();
            //date
            string tempDuration = durationTextBox.Text.ToString();
            string tempPrice = priceTextBox.Text.ToString();

            btnNewFlight.IsEnabled = false;
            btnEditFlight.IsEnabled = false;
            btnDeleteFlight.IsEnabled = false;

            btnSaveFlight.IsEnabled = true;
            btnCancelFlight.IsEnabled = true;

            flightDataGrid.IsEnabled = false;

            btnPrevFlight.IsEnabled = false;
            btnNextFlight.IsEnabled = false;

            fromTextBox.IsEnabled = true;
            toTextBox.IsEnabled = true;
            //date
            durationTextBox.IsEnabled = true;
            priceTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(fromTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(toTextBox, TextBox.TextProperty);
            //date
            BindingOperations.ClearBinding(durationTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(priceTextBox, TextBox.TextProperty);
            fromTextBox.Text = tempFrom;
            toTextBox.Text = tempTo;
            //date
            durationTextBox.Text = tempDuration;
            priceTextBox.Text = tempPrice;
            Keyboard.Focus(fromTextBox);

            //SetValidationBinding();
        }

        private void btnDeleteFlight_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFrom = fromTextBox.Text.ToString();
            string tempTo = toTextBox.Text.ToString();
            //date
            string tempDuration = durationTextBox.Text.ToString();
            string tempPrice = priceTextBox.Text.ToString();

            btnNewFlight.IsEnabled = false;
            btnEditFlight.IsEnabled = false;
            btnDeleteFlight.IsEnabled = false;

            btnSaveFlight.IsEnabled = true;
            btnCancelFlight.IsEnabled = true;

            flightDataGrid.IsEnabled = false;

            btnPrevFlight.IsEnabled = false;
            btnNextFlight.IsEnabled = false;

            fromTextBox.IsEnabled = false;
            toTextBox.IsEnabled = false;
            //date
            emailTextBox.IsEnabled = false;
            nationalityTextBox.IsEnabled = false;

            BindingOperations.ClearBinding(fromTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(toTextBox, TextBox.TextProperty);
            //date
            BindingOperations.ClearBinding(durationTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(priceTextBox, TextBox.TextProperty);
            fromTextBox.Text = tempFrom;
            toTextBox.Text = tempTo;
            //date
            durationTextBox.Text = tempDuration;
            priceTextBox.Text = tempPrice;

        }

        private void btnCancelFlight_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNewFlight.IsEnabled = true;
            btnEditFlight.IsEnabled = true;
            btnDeleteFlight.IsEnabled = true;

            btnSaveFlight.IsEnabled = false;
            btnCancelFlight.IsEnabled = false;

            flightDataGrid.IsEnabled = true;

            btnPrevFlight.IsEnabled = true;
            btnNextFlight.IsEnabled = true;

            fromTextBox.IsEnabled = false;
            toTextBox.IsEnabled = false;
            //date
            durationTextBox.IsEnabled = false;
            priceTextBox.IsEnabled = false;

            fromTextBox.SetBinding(TextBox.TextProperty, txtFromBinding);
            toTextBox.SetBinding(TextBox.TextProperty, txtToBinding);
            //date
            durationTextBox.SetBinding(TextBox.TextProperty, txtDurationBinding);
            priceTextBox.SetBinding(TextBox.TextProperty, txtPriceBinding);
        }

        private void btnSaveBooked_Click(object sender, RoutedEventArgs e)
        {
            BookedFlight booked = null;
            if (action == ActionState.New)
            {
                try
                {
                    Client client = (Client)cmbClients.SelectedItem;
                    Flight flight = (Flight)cmbFlights.SelectedItem;
                    //instantiem Order entity
                    booked = new BookedFlight()
                    {
                        ClientId = client.Id,
                        FlightId = flight.Id
                    };
                    //adaugam entitatea nou creata in context
                    ctx.BookedFlights.Add(booked);
                    clientBookedFlightsViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }



                btnNewBooked.IsEnabled = true;
                btnEditBooked.IsEnabled = true;
                btnDeleteBooked.IsEnabled = true;

                btnSaveBooked.IsEnabled = false;
                btnCancelBooked.IsEnabled = false;

                bookedFlightsDataGrid.IsEnabled = true;

                btnPreviousBooked.IsEnabled = true;
                btnNextBooked.IsEnabled = true;

                cmbClients.IsEnabled = true;
                cmbFlights.IsEnabled = true;


            }
            else
            {
                if (action == ActionState.Edit)
                {
                    dynamic selectedBookedFlight = bookedFlightsDataGrid.SelectedItem;
                    try
                    {
                        int curr_id = selectedBookedFlight.Id;
                        var editedBookedFlight = ctx.BookedFlights.FirstOrDefault(s => s.Id == curr_id);
                        if (editedBookedFlight != null)
                        {
                            editedBookedFlight.ClientId = Int32.Parse(cmbClients.SelectedValue.ToString());
                            editedBookedFlight.FlightId = Convert.ToInt32(cmbFlights.SelectedValue.ToString());
                            //salvam modificarile
                            ctx.SaveChanges();
                        }
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                 
                    // pozitionarea pe item-ul curent
                    clientViewSource.View.MoveCurrentTo(selectedBookedFlight);

                    btnNewBooked.IsEnabled = true;
                    btnEditBooked.IsEnabled = true;
                    btnDeleteBooked.IsEnabled = true;

                    btnSaveBooked.IsEnabled = false;
                    btnCancelBooked.IsEnabled = false;

                    bookedFlightsDataGrid.IsEnabled = true;

                    btnPreviousBooked.IsEnabled = true;
                    btnNextBooked.IsEnabled = true;

                    cmbClients.IsEnabled = false;
                    cmbFlights.IsEnabled = false;

                }



                else if (action == ActionState.Delete)
                {
                    try
                    {
                        dynamic selectedBookedFlight = bookedFlightsDataGrid.SelectedItem;
                        int curr_id = selectedBookedFlight.Id;
                        var deletedBookedFlight = ctx.BookedFlights.FirstOrDefault(s => s.Id == curr_id);
                        if (deletedBookedFlight != null)
                        {
                            ctx.BookedFlights.Remove(deletedBookedFlight);
                            ctx.SaveChanges();
                            MessageBox.Show("Reservation Deleted Successfully", "Message");
                           
                        }
                    }
                    catch (DataException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    btnNewBooked.IsEnabled = true;
                    btnEditBooked.IsEnabled = true;
                    btnDeleteBooked.IsEnabled = true;

                    btnSaveBooked.IsEnabled = false;
                    btnCancelBooked.IsEnabled = false;

                    bookedFlightsDataGrid.IsEnabled = true;

                    btnPreviousBooked.IsEnabled = true;
                    btnNextBooked.IsEnabled = true;

                    cmbClients.IsEnabled = false;
                    cmbFlights.IsEnabled = false;

                    //SetValidationBinding();
                }

            }
        }

        private void btnNextBooked_Click(object sender, RoutedEventArgs e)
        {
            clientBookedFlightsViewSource.View.MoveCurrentToNext();
        }
        private void btnPreviousBooked_Click(object sender, RoutedEventArgs e)
        {
            clientBookedFlightsViewSource.View.MoveCurrentToPrevious();
        }
        private void btnNewBooked_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewBooked.IsEnabled = false;
            btnEditBooked.IsEnabled = false;
            btnDeleteBooked.IsEnabled = false;

            btnSaveBooked.IsEnabled = true;
            btnCancelBooked.IsEnabled = true;

            bookedFlightsDataGrid.IsEnabled = false;

            btnPreviousBooked.IsEnabled = false;
            btnNextBooked.IsEnabled = false;

            cmbClients.IsEnabled = true;
            cmbFlights.IsEnabled = true;

            //SetValidationBinding();

            BindingOperations.ClearBinding(cmbClients, ComboBox.TextProperty);
            BindingOperations.ClearBinding(cmbFlights, ComboBox.TextProperty);
            cmbClients.Text = "";
            cmbFlights.Text = "";
            Keyboard.Focus(cmbClients);

        }

        private void btnEditBooked_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            btnNewBooked.IsEnabled = false;
            btnEditBooked.IsEnabled = false;
            btnDeleteBooked.IsEnabled = false;

            btnSaveBooked.IsEnabled = true;
            btnCancelBooked.IsEnabled = true;

            bookedFlightsDataGrid.IsEnabled = false;

            btnPreviousBooked.IsEnabled = false;
            btnNextBooked.IsEnabled = false;

            cmbClients.IsEnabled = true;
            cmbFlights.IsEnabled = true;

            //SetValidationBinding();

            BindingOperations.ClearBinding(cmbClients, ComboBox.TextProperty);
            BindingOperations.ClearBinding(cmbFlights, ComboBox.TextProperty);
            Keyboard.Focus(cmbClients);

        }

        private void btnDeleteBooked_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            btnNewBooked.IsEnabled = false;
            btnEditBooked.IsEnabled = false;
            btnDeleteBooked.IsEnabled = false;

            btnSaveBooked.IsEnabled = true;
            btnCancelBooked.IsEnabled = true;

            bookedFlightsDataGrid.IsEnabled = false;

            btnPreviousBooked.IsEnabled = false;
            btnNextBooked.IsEnabled = false;

            cmbClients.IsEnabled = false;
            cmbFlights.IsEnabled = false;

            BindingOperations.ClearBinding(cmbClients, ComboBox.TextProperty);
            BindingOperations.ClearBinding(cmbFlights, ComboBox.TextProperty);

        }

        private void btnCancelBooked_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNewBooked.IsEnabled = true;
            btnEditBooked.IsEnabled = true;
            btnDeleteBooked.IsEnabled = true;

            btnSaveBooked.IsEnabled = false;
            btnCancelBooked.IsEnabled = false;

            bookedFlightsDataGrid.IsEnabled = true;

            btnPreviousBooked.IsEnabled = true;
            btnNextBooked.IsEnabled = true;

            cmbClients.IsEnabled = false;
            cmbFlights.IsEnabled = false;


        }

        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = clientViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = clientViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            //lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValid());
            //lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding); //setare binding nou
        }

    }
}
