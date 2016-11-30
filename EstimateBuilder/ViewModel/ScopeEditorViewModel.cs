using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ScopeEditorViewModel : ViewModelBase, IDropTarget
    {
        //Initializer
        public ScopeEditorViewModel()
        {
            Bid = new TECBid();
            Templates = new TECTemplates();
            ViewData = new ScopeEditorViewModelData();
            DataGridVisibilty = new VisibilityModel();

            populateItemsCollections();
            
            AddTagCommand = new RelayCommand(AddTagExecute);
            AddTagToSystemCommand = new RelayCommand(AddTagToSystemExecute);
            AddTagToEquipmentCommand = new RelayCommand(AddTagToEquipmentExecute);
            AddTagToSubScopeCommand = new RelayCommand(AddTagToSubScopeExecute);
            AddTagToPointCommand = new RelayCommand(AddTagToPointExecute);
            AddPointCommand = new RelayCommand(AddPointExecute, AddPointCanExecute);
            SearchCollectionCommand = new RelayCommand(SearchCollectionExecute);
            EndSearchCommand = new RelayCommand(EndSearchExecute);

            DeleteSelectedSystemCommand = new RelayCommand(DeleteSelectedSystemExecute);
            DeleteSelectedEquipmentCommand = new RelayCommand(DeleteSelectedEquipmentExecute);
            DeleteSelectedSubScopeCommand = new RelayCommand(DeleteSelectedSubScopeExecute);
            DeleteSelectedPointCommand = new RelayCommand(DeleteSelectedPointExecute);

            setVisibility(0);

            MessengerInstance.Register<GenericMessage<TECBid>>(this, PopulateBid);
            MessengerInstance.Register<GenericMessage<TECTemplates>>(this, PopulateTemplates);

            MessengerInstance.Send<NotificationMessage>(new NotificationMessage("ScopeEditorViewModelLoaded"));
        }

        #region Properties
        #region Interface Properties
        public ScopeEditorViewModelData ViewData
        {
            get { return _viewData; }
            set
            {
                _viewData = value;
                RaisePropertyChanged("ViewData");
            }
        }
        private ScopeEditorViewModelData _viewData;

        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibility; }
            set
            {
                _dataGridVisibility = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }
        private VisibilityModel _dataGridVisibility;

        #region Scope Properties
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }
        private TECBid _bid;
        #endregion Scope Properties

        #region Selected Object Properties


        public TECSystem SelectedSystem
        {
            get
            {
                return _selectedSystem;
            }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                RightTabIndex = EditIndex.System;
            }
        }
        private TECSystem _selectedSystem;

        public TECEquipment SelectedEquipment
        {
            get
            {
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                RightTabIndex = EditIndex.Equipment;
                
            }
        }
        private TECEquipment _selectedEquipment;

        public TECSubScope SelectedSubScope
        {
            get
            {
                return _selectedSubScope;
            }
            set
            {
                _selectedSubScope = value;
                RaisePropertyChanged("SelectedSubscope");
                RightTabIndex = EditIndex.SubScope;
            }
        }
        private TECSubScope _selectedSubScope;

        public TECDevice SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
                RightTabIndex = EditIndex.Device;
            }
        }
        private TECDevice _selectedDevice;

        public TECPoint SelectedPoint
        {
            get { return _selectedPoint; }
            set
            {
                _selectedPoint = value;
                RaisePropertyChanged("SelectedPoint");
                RightTabIndex = EditIndex.Point;
            }
        }
        private TECPoint _selectedPoint;

        public TECTag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
                RaisePropertyChanged("SelectedTag");
            }
        }
        private TECTag _selectedTag;
        #endregion //Selected Object Properties

        #region Tab Indexes
        public GridIndex DGTabIndex
        {
            get
            {
                return _DGTabIndex;
            }
            set
            {
                _DGTabIndex = value;
                setVisibility(_DGTabIndex);
                RaisePropertyChanged("DGTabIndex");
            }
        }
        private GridIndex _DGTabIndex;

        public AddIndex LeftTabIndex
        {
            get { return _leftTabIndex; }
            set
            {
                _leftTabIndex = value;
                RaisePropertyChanged("LeftTabIndex");
            }
        }
        private AddIndex _leftTabIndex;

        public EditIndex RightTabIndex
        {
            get { return _rightTabIndex; }
            set
            {
                _rightTabIndex = value;
                RaisePropertyChanged("RightTabIndex");
            }
        }
        private EditIndex _rightTabIndex;
        #endregion //Tab Indexes

        #region Visibility Properties
        public Visibility LeftSystemsVisibility
        {
            get { return _leftSystemsVisibility; }
            set
            {
                _leftSystemsVisibility = value;
                RaisePropertyChanged("LeftSystemsVisibility");
            }
        }
        private Visibility _leftSystemsVisibility;

        public Visibility LeftEquipmentVisibility
        {
            get { return _leftEquipmentVisibility; }
            set
            {
                _leftEquipmentVisibility = value;
                RaisePropertyChanged("LeftEquipmentVisibility");
            }
        }
        private Visibility _leftEquipmentVisibility;

        public Visibility LeftSubScopeVisibility
        {
            get { return _leftSubScopeVisibility; }
            set
            {
                _leftSubScopeVisibility = value;
                RaisePropertyChanged("LeftSubScopeVisibility");
            }
        }
        private Visibility _leftSubScopeVisibility;

        public Visibility LeftDevicesVisibility
        {
            get { return _leftDevicesVisibility; }
            set
            {
                _leftDevicesVisibility = value;
                RaisePropertyChanged("LeftDevicesVisibility");
            }
        }
        private Visibility _leftDevicesVisibility;

        public Visibility LeftDevicesEditVisibility
        {
            get { return _leftDevicesEditVisibility; }
            set
            {
                _leftDevicesEditVisibility = value;
                RaisePropertyChanged("LeftDevicesEditVisibility");
            }
        }
        private Visibility _leftDevicesEditVisibility;

        public Visibility LeftManufacturerVisibility
        {
            get { return _leftManufacturerVisibility; }
            set
            {
                _leftManufacturerVisibility = value;
                RaisePropertyChanged("LeftManufacturerVisibility");
            }
        }
        private Visibility _leftManufacturerVisibility;
        #endregion //Visibility Properties

        #region Point Interface Properties
        public string PointName
        {
            get { return _pointName; }
            set
            {
                _pointName = value;
                RaisePropertyChanged("PointName");
            }
        }
        private string _pointName;

        public string PointDescription
        {
            get { return _pointDescription; }
            set
            {
                _pointDescription = value;
                RaisePropertyChanged("PointDescription");
            }
        }
        private string _pointDescription;

        public PointTypes PointType
        {
            get { return _pointType; }
            set
            {
                _pointType = value;
                RaisePropertyChanged("PointType");
            }
        }
        private PointTypes _pointType;
        
        public int PointQuantity
        {
            get { return _pointQuantity; }
            set
            {
                _pointQuantity = value;
                RaisePropertyChanged("PointQuantity");
            }
        }
        private int _pointQuantity;
        #endregion //Point Interface Properties

        public string TagName
        {
            get { return _tagName; }
            set
            {
                _tagName = value;
                RaisePropertyChanged("TagName");
            }
        }
        private string _tagName;
        #endregion //Interface Properties

        #region Commands Properties
        public ICommand AddDeviceCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        public ICommand AddTagToSystemCommand { get; private set; }
        public ICommand AddTagToEquipmentCommand { get; private set; }
        public ICommand AddTagToSubScopeCommand { get; private set; }
        public ICommand AddTagToPointCommand { get; private set; }
        public ICommand AddPointCommand { get; private set; }
        public ICommand SearchCollectionCommand { get; private set; }
        public ICommand EndSearchCommand { get; private set; }
        public ICommand DeleteSelectedSystemCommand { get; private set; }
        public ICommand DeleteSelectedEquipmentCommand { get; private set; }
        public ICommand DeleteSelectedSubScopeCommand { get; private set; }
        public ICommand DeleteSelectedPointCommand { get; private set; }
        #endregion //Commands Properties
        #endregion //Properties

        #region Methods
        #region Commands Methods
        private void AddTagExecute()
        {
            TECTag newTag = new TECTag();
            newTag.Text = TagName;
            Templates.Tags.Add(newTag);
        }
        private void AddTagToSystemExecute()
        {
            if (SelectedTag != null && SelectedSystem != null)
            {
                SelectedSystem.Tags.Add(SelectedTag);
            }
        }
        private void AddTagToEquipmentExecute()
        {
            if (SelectedTag != null && SelectedEquipment != null)
            {
                SelectedEquipment.Tags.Add(SelectedTag);
            }

        }
        private void AddTagToSubScopeExecute()
        {
            if (SelectedTag != null && SelectedSubScope != null)
            {
                SelectedSubScope.Tags.Add(SelectedTag);
            }
        }
        private void AddTagToPointExecute()
        {
            if (SelectedTag != null && SelectedPoint != null)
            {
                SelectedPoint.Tags.Add(SelectedTag);
            }
        }

        private void AddPointExecute()
        {
            Console.WriteLine(PointQuantity);
            TECPoint newPoint = new TECPoint();
            newPoint.Name = PointName;
            newPoint.Description = PointDescription;
            newPoint.Type = PointType;
            newPoint.Quantity = PointQuantity;
            if (PointType != 0)
            {
                SelectedSubScope.Points.Add(newPoint);
            }
        }
        private bool AddPointCanExecute()
        {
            if ((PointType != 0) && (PointName != ""))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void SearchCollectionExecute()
        {
            if (ViewData.SearchString != null)
            {
                switch (LeftTabIndex)
                {
                    case AddIndex.System:
                        ViewData.SystemItemsCollection = new ObservableCollection<TECSystem>();
                        foreach (TECSystem item in Templates.SystemTemplates)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.SystemItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.SystemItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case AddIndex.Equipment:
                        ViewData.EquipmentItemsCollection = new ObservableCollection<TECEquipment>();
                        foreach (TECEquipment item in Templates.EquipmentTemplates)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.EquipmentItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.EquipmentItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case AddIndex.SubScope:
                        ViewData.SubScopeItemsCollection = new ObservableCollection<TECSubScope>();
                        foreach (TECSubScope item in Templates.SubScopeTemplates)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.SubScopeItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.SubScopeItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    case AddIndex.Tags:
                        ViewData.DevicesItemsCollection = new ObservableCollection<TECDevice>();
                        foreach (TECDevice item in Templates.DeviceCatalog)
                        {
                            if (item.Name.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                            {
                                Console.WriteLine(item.Name);
                                ViewData.DevicesItemsCollection.Add(item);
                            }
                            foreach (TECTag tag in item.Tags)
                            {
                                if (tag.Text.ToUpper().Contains(ViewData.SearchString.ToUpper()))
                                {
                                    ViewData.DevicesItemsCollection.Add(item);
                                }
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
        }
        private void AddManufacturerExecute()
        {
            TECManufacturer newMan = new TECManufacturer(ViewData.ManufacturerName, "", ViewData.ManufacturerMultiplier);
            Templates.ManufacturerCatalog.Add(newMan);
        }

        private void DeleteSelectedSystemExecute()
        {
            Templates.SystemTemplates.Remove(SelectedSystem);
            SelectedSystem = null;
            resetEditTab();
        }
        private void DeleteSelectedEquipmentExecute()
        {
            if (SelectedSystem != null)
            {
                SelectedSystem.Equipment.Remove(SelectedEquipment);
            }
            else
            {
                Templates.EquipmentTemplates.Remove(SelectedEquipment);
            }
            SelectedEquipment = null;
            resetEditTab();
        }
        private void DeleteSelectedSubScopeExecute()
        {
            if (SelectedEquipment != null)
            {
                SelectedEquipment.SubScope.Remove(SelectedSubScope);
            }
            else
            {
                Templates.SubScopeTemplates.Remove(SelectedSubScope);
            }
            SelectedSubScope = null;
            resetEditTab();
        }
        private void DeleteSelectedPointExecute()
        {
            if (SelectedSubScope != null)
            {
                SelectedSubScope.Points.Remove(SelectedPoint);
            }
            SelectedPoint = null;
            resetEditTab();
        }

        private void EndSearchExecute()
        {
            populateItemsCollections();
            ViewData.SearchString = "";
        }
        #endregion //Commands Methods

        #region Message Methods

        public void PopulateBid(GenericMessage<TECBid> genericMessage)
        {
            Bid = genericMessage.Content;
        }

        public void PopulateTemplates(GenericMessage<TECTemplates> genericMessage)
        {
            Templates = genericMessage.Content;

            populateItemsCollections();
        }

        #endregion Message Methods

        #region Drag Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;
            Type sourceType = sourceItem.GetType();
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (sourceItem != null && sourceType == targetType)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            Object sourceItem;
            if (dropInfo.Data is TECDevice)
            {
                Console.WriteLine("Is Device");
                sourceItem = new TECDevice((TECDevice)dropInfo.Data);
            }
            else
            {
                Console.WriteLine("Is of type: " + dropInfo.Data.GetType());
                sourceItem = dropInfo.Data;
            }

            if (dropInfo.InsertIndex > dropInfo.DragInfo.SourceIndex)
            {
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
                if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }

            }
            else
            {
                if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }
                ((IList)dropInfo.TargetCollection).Add(sourceItem);
            }

        }
        #endregion

        #region Helper Methods


        private void setVisibility(GridIndex gridIndex)
        {
            nullifySelected();
            /*
            switch (gridIndex)
            {
                case GridIndex.Systems:
                    LeftTabIndex = 0;

                    LeftSystemsVisibility = Visibility.Visible;
                    LeftEquipmentVisibility = Visibility.Visible;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;

                    DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    break;
                case GridIndex.Equip:
                    LeftTabIndex = 1;

                    LeftSystemsVisibility = Visibility.Collapsed;
                    LeftEquipmentVisibility = Visibility.Visible;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;

                    DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
                    DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    break;
                case 2:
                    LeftTabIndex = 2;

                    LeftSystemsVisibility = Visibility.Collapsed;
                    LeftEquipmentVisibility = Visibility.Collapsed;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;

                    DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
                    break;
                case 3:
                    LeftTabIndex = 4;

                    LeftSystemsVisibility = Visibility.Collapsed;
                    LeftEquipmentVisibility = Visibility.Collapsed;
                    LeftSubScopeVisibility = Visibility.Collapsed;
                    LeftDevicesVisibility = Visibility.Collapsed;
                    LeftDevicesEditVisibility = Visibility.Visible;
                    LeftManufacturerVisibility = Visibility.Visible;
                    break;
                default:
                    LeftTabIndex = 0;

                    LeftSystemsVisibility = Visibility.Visible;
                    LeftEquipmentVisibility = Visibility.Visible;
                    LeftSubScopeVisibility = Visibility.Visible;
                    LeftDevicesVisibility = Visibility.Visible;
                    LeftDevicesEditVisibility = Visibility.Collapsed;
                    LeftManufacturerVisibility = Visibility.Collapsed;
                    break;

            }
            */
        }

        private void nullifySelected()
        {
            SelectedDevice = null;
            SelectedPoint = null;
            SelectedSubScope = null;
            SelectedEquipment = null;
            SelectedSystem = null;
            resetEditTab();
        }

        private void populateItemsCollections()
        {
            ViewData.SystemItemsCollection = Templates.SystemTemplates;
            ViewData.EquipmentItemsCollection = Templates.EquipmentTemplates;
            ViewData.SubScopeItemsCollection = Templates.SubScopeTemplates;
            ViewData.DevicesItemsCollection = Templates.DeviceCatalog;
        }

        private void resetEditTab()
        {
            RightTabIndex = (EditIndex)DGTabIndex;
        }
        #endregion //Helper Methods
        #endregion //Methods
    }
}