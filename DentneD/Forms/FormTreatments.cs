﻿#region License
// Copyright (c) 2015 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DG.Data.Model.Helpers;
using DG.UI.GHF;
using DG.DentneD.Model;
using DG.DentneD.Model.Entity;
using DG.DentneD.Forms.Objects;
using System.Data;
using Zuby.ADGV;
using SMcMaster;

namespace DG.DentneD.Forms
{
    public partial class FormTreatments : DGUIGHFForm
    {
        private DentneDModel _dentnedModel = null;

        private TabElement tabElement_tabTreatments = new TabElement();
        private TabElement tabElement_tabTreatmentsPrices = new TabElement();

        /// <summary>
        /// Constructor
        /// </summary>
        public FormTreatments()
        {
            InitializeComponent();
            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.AcrossFirst);

            Initialize(Program.uighfApplication);

            _dentnedModel = new DentneDModel();
            _dentnedModel.LanguageHelper.LoadFromFile(Program.uighfApplication.LanguageFilename);
        }

        /// <summary>
        /// Add components language
        /// </summary>
        public override void AddLanguageComponents()
        {
            //main
            LanguageHelper.AddComponent(this);
            LanguageHelper.AddComponent(treatmentsidDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            LanguageHelper.AddComponent(nameDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            LanguageHelper.AddComponent(codeDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            LanguageHelper.AddComponent(typeDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            //tabTreatments
            LanguageHelper.AddComponent(tabPage_tabTreatments);
            LanguageHelper.AddComponent(button_tabTreatments_new);
            LanguageHelper.AddComponent(button_tabTreatments_edit);
            LanguageHelper.AddComponent(button_tabTreatments_delete);
            LanguageHelper.AddComponent(button_tabTreatments_save);
            LanguageHelper.AddComponent(button_tabTreatments_cancel);
            LanguageHelper.AddComponent(treatments_idLabel);
            LanguageHelper.AddComponent(treatments_nameLabel);
            LanguageHelper.AddComponent(treatments_codeLabel);
            LanguageHelper.AddComponent(treatmentstypes_idLabel);
            LanguageHelper.AddComponent(treatments_mexpirationLabel);
            LanguageHelper.AddComponent(treatments_mexpirationinfoLabel);
            LanguageHelper.AddComponent(treatments_priceLabel);
            LanguageHelper.AddComponent(treatments_notesLabel);
            LanguageHelper.AddComponent(taxes_idLabel);
            LanguageHelper.AddComponent(button_tabTreatments_unsettaxesid);
            //tabTreatmentsPrices
            LanguageHelper.AddComponent(tabPage_tabTreatmentsPrices);
            LanguageHelper.AddComponent(label_tabTreatmentsPrices_filterpriceslists);
            LanguageHelper.AddComponent(treatmentspricesidDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            LanguageHelper.AddComponent(pricelistDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            LanguageHelper.AddComponent(priceDataGridViewTextBoxColumn, this.GetType().Name, "HeaderText");
            LanguageHelper.AddComponent(button_tabTreatmentsPrices_new);
            LanguageHelper.AddComponent(button_tabTreatmentsPrices_edit);
            LanguageHelper.AddComponent(button_tabTreatmentsPrices_delete);
            LanguageHelper.AddComponent(button_tabTreatmentsPrices_save);
            LanguageHelper.AddComponent(button_tabTreatmentsPrices_cancel);
            LanguageHelper.AddComponent(treatmentsprices_idLabel);
            LanguageHelper.AddComponent(treatmentspriceslists_idLabel);
            LanguageHelper.AddComponent(treatmentsprices_priceLabel);
        }

        /// <summary>
        /// Initialize TabElements
        /// </summary>
        protected override void InitializeTabElements()
        {
            //set Readonly OnSetEditingMode for Controls
            DisableReadonlyCheckOnSetEditingModeControlCollection.Add(typeof(DataGridView));
            DisableReadonlyCheckOnSetEditingModeControlCollection.Add(typeof(AdvancedDataGridView));

            //set Main BindingSource
            BindingSourceMain = vTreatmentsBindingSource;
            GetDataSourceMain = GetDataSource_main;

            //set Main TabControl
            TabControlMain = tabControl_main;

            //set Main Panels
            PanelFiltersMain = panel_filters;
            PanelListMain = panel_list;
            PanelsExtraMain = null;

            //set tabTreatments
            tabElement_tabTreatments = new TabElement()
            {
                TabPageElement = tabPage_tabTreatments,
                ElementItem = new TabElement.TabElementItem()
                {
                    PanelData = panel_tabTreatments_data,
                    PanelActions = panel_tabTreatments_actions,
                    PanelUpdates = panel_tabTreatments_updates,

                    BindingSourceList = vTreatmentsBindingSource,
                    GetDataSourceList = GetDataSource_main,

                    BindingSourceEdit = treatmentsBindingSource,
                    GetDataSourceEdit = GetDataSourceEdit_tabTreatments,
                    AfterSaveAction = AfterSaveAction_tabTreatments,

                    AddButton = button_tabTreatments_new,
                    UpdateButton = button_tabTreatments_edit,
                    RemoveButton = button_tabTreatments_delete,
                    SaveButton = button_tabTreatments_save,
                    CancelButton = button_tabTreatments_cancel,

                    Add = Add_tabTreatments,
                    Update = Update_tabTreatments,
                    Remove = Remove_tabTreatments
                }
            };

            //set tabTreatmentsPrices
            tabElement_tabTreatmentsPrices = new TabElement()
            {
                TabPageElement = tabPage_tabTreatmentsPrices,
                ElementListItem = new TabElement.TabElementListItem()
                {
                    PanelFilters = panel_tabTreatmentsPrices_filters,
                    PanelList = panel_tabTreatmentsPrices_list,

                    PanelData = panel_tabTreatmentsPrices_data,
                    PanelActions = panel_tabTreatmentsPrices_actions,
                    PanelUpdates = panel_tabTreatmentsPrices_updates,

                    BindingSourceList = vTreatmentsPricesBindingSource,
                    GetDataSourceList = GetDataSourceList_tabTreatmentsPrices,

                    BindingSourceEdit = treatmentspricesBindingSource,
                    GetDataSourceEdit = GetDataSourceEdit_tabTreatmentsPrices,
                    AfterSaveAction = AfterSaveAction_tabTreatmentsPrices,
                    
                    AddButton = button_tabTreatmentsPrices_new,
                    IsAddButtonDefaultClickEventAttached = false,
                    UpdateButton = button_tabTreatmentsPrices_edit,
                    RemoveButton = button_tabTreatmentsPrices_delete,
                    SaveButton = button_tabTreatmentsPrices_save,
                    CancelButton = button_tabTreatmentsPrices_cancel,

                    Add = Add_tabTreatmentsPrices,
                    Update = Update_tabTreatmentsPrices,
                    Remove = Remove_tabTreatmentsPrices
                }
            };

            //set Elements
            TabElements.Add(tabElement_tabTreatments);
            TabElements.Add(tabElement_tabTreatmentsPrices);
        }

        /// <summary>
        /// Loader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTreatments_Load(object sender, EventArgs e)
        {
            IsBindingSourceLoading = true;
            advancedDataGridView_main.SortASC(advancedDataGridView_main.Columns[1]);
            IsBindingSourceLoading = false;

            PreloadView();

            ReloadView();
        }

        /// <summary>
        /// Preload View
        /// </summary>
        private void PreloadView()
        {
            //load categories
            treatmentstypes_idComboBox.DataSource = _dentnedModel.TreatmentsTypes.List().OrderBy(r => r.treatmentstypes_name).ToList();
            treatmentstypes_idComboBox.DisplayMember = "treatmentstypes_name";
            treatmentstypes_idComboBox.ValueMember = "treatmentstypes_id";

            //load prices lists
            treatmentspriceslists_idComboBox.DataSource = _dentnedModel.TreatmentsPricesLists.List().OrderBy(r => r.treatmentspriceslists_name).ToList();
            treatmentspriceslists_idComboBox.DisplayMember = "treatmentspriceslists_name";
            treatmentspriceslists_idComboBox.ValueMember = "treatmentspriceslists_id";

            //load prices lists
            comboBox_tabTreatmentsPrices_filterPriceslists.Items.Clear();
            comboBox_tabTreatmentsPrices_filterPriceslists.Items.Add(new DGUIGHFUtilsUI.DGComboBoxItem("-1", ""));
            foreach (treatmentspriceslists a in _dentnedModel.TreatmentsPricesLists.List().OrderBy(r => r.treatmentspriceslists_name))
            {
                comboBox_tabTreatmentsPrices_filterPriceslists.Items.Add(new DGUIGHFUtilsUI.DGComboBoxItem(a.treatmentspriceslists_id.ToString(), a.treatmentspriceslists_name));
            }
            comboBox_tabTreatmentsPrices_filterPriceslists.SelectedIndex = -1;

            //load tax rates
            taxes_idComboBox.DataSource = _dentnedModel.Taxes.List().OrderBy(r => r.taxes_name).ToList();
            taxes_idComboBox.DisplayMember = "taxes_name";
            taxes_idComboBox.ValueMember = "taxes_id";
        }

        /// <summary>
        /// Reset all the tab datagrid
        /// </summary>
        private void ResetTabsDataGrid()
        {
            IsBindingSourceLoading = true;
            advancedDataGridView_tabTreatmentsPrices_list.CleanFilterAndSort();
            advancedDataGridView_tabTreatmentsPrices_list.SortASC(advancedDataGridView_tabTreatmentsPrices_list.Columns[1]);
            IsBindingSourceLoading = false;
        }

        /// <summary>
        /// Get main list DataSource
        /// </summary>
        /// <returns></returns>
        private object GetDataSource_main()
        {
            ResetTabsDataGrid();

            IEnumerable<VTreatments> vTreatments =
                _dentnedModel.Treatments.List().Select(
                r => new VTreatments
                {
                    treatments_id = r.treatments_id,
                    code = r.treatments_code,
                    type = _dentnedModel.TreatmentsTypes.Find(r.treatmentstypes_id).treatmentstypes_name,
                    name = r.treatments_name
                }).ToList();

            return DGDataTableUtils.ToDataTable<VTreatments>(vTreatments);
        }

        /// <summary>
        /// Main Datagrid filter handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advancedDataGridView_main_FilterStringChanged(object sender, EventArgs e)
        {
            vTreatmentsBindingSource.Filter = advancedDataGridView_main.FilterString;
        }

        /// <summary>
        /// Main Datagrid sort handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advancedDataGridView_main_SortStringChanged(object sender, EventArgs e)
        {
            vTreatmentsBindingSource.Sort = advancedDataGridView_main.SortString;
        }

        /// <summary>
        /// Main list current element changed hanlder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vTreatmentsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (IsBindingSourceLoading)
                return;

            //get current itme
            int treatments_id = -1;
            if (vTreatmentsBindingSource.Current != null)
            {
                treatments_id = (((DataRowView)vTreatmentsBindingSource.Current).Row).Field<int>("treatments_id");
            }

            //set treatments fields
            treatments_mexpirationTextBox.Text = "";
            if (treatments_id != -1)
            {
                treatments treatment = _dentnedModel.Treatments.Find(treatments_id);
                treatments_mexpirationTextBox.Text = treatment.treatments_mexpiration.ToString();
            }

            //reset treatments prices filter
            comboBox_tabTreatmentsPrices_filterPriceslists.SelectedIndex = -1;
        }


        #region tabTreatments

        /// <summary>
        /// Load the tab DataSource
        /// </summary>
        /// <returns></returns>
        private object GetDataSourceEdit_tabTreatments()
        {
            return DGUIGHFData.LoadEntityFromCurrentBindingSource<treatments, DentneDModel>(_dentnedModel.Treatments, vTreatmentsBindingSource, new string[] { "treatments_id" });
        }

        /// <summary>
        /// Do actions after Save
        /// </summary>
        /// <param name="item"></param>
        private void AfterSaveAction_tabTreatments(object item)
        {
            DGUIGHFData.SetBindingSourcePosition<treatments, DentneDModel>(_dentnedModel.Treatments, item, vTreatmentsBindingSource);
        }

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item"></param>
        private void Add_tabTreatments(object item)
        {
            //update mexpiration
            if (!String.IsNullOrEmpty(treatments_mexpirationTextBox.Text))
                ((treatments)item).treatments_mexpiration = Convert.ToByte(treatments_mexpirationTextBox.Text);
            else
                ((treatments)item).treatments_mexpiration = null;

            DGUIGHFData.Add<treatments, DentneDModel>(_dentnedModel.Treatments, item);
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="item"></param>
        private void Update_tabTreatments(object item)
        {
            //update mexpiration
            if (!String.IsNullOrEmpty(treatments_mexpirationTextBox.Text))
                ((treatments)item).treatments_mexpiration = Convert.ToByte(treatments_mexpirationTextBox.Text);
            else
                ((treatments)item).treatments_mexpiration = null;

            DGUIGHFData.Update<treatments, DentneDModel>(_dentnedModel.Treatments, item);
        }

        /// <summary>
        /// Remove an item
        /// </summary>
        /// <param name="item"></param>
        private void Remove_tabTreatments(object item)
        {
            DGUIGHFData.Remove<treatments, DentneDModel>(false, _dentnedModel.Treatments, item);
        }

        /// <summary>
        /// Unset taxes_id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_tabTreatments_unsettaxesid_Click(object sender, EventArgs e)
        {
            taxes_idComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Combobox autocomplete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treatmentstypes_idComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DGUIGHFUtilsUI.DGComboBoxAutoComplete.OnKeyPress((ComboBox)sender, e);
        }

        /// <summary>
        /// Combobox autocomplete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taxes_idComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DGUIGHFUtilsUI.DGComboBoxAutoComplete.OnKeyPress((ComboBox)sender, e);
        }

        #endregion


        #region tabTreatmentsPrices

        /// <summary>
        /// Get tab list DataSource
        /// </summary>
        /// <returns></returns>
        private object GetDataSourceList_tabTreatmentsPrices()
        {
            object ret = null;

            //get current treatment
            int treatments_id = -1;
            if (vTreatmentsBindingSource.Current != null)
            {
                treatments_id = (((DataRowView)vTreatmentsBindingSource.Current).Row).Field<int>("treatments_id");
            }

            //get treatments
            List<treatmentsprices> treatmentspricesl = new List<treatmentsprices>();
            if (comboBox_tabTreatmentsPrices_filterPriceslists.SelectedIndex != -1 && comboBox_tabTreatmentsPrices_filterPriceslists.SelectedIndex != 0)
                treatmentspricesl = _dentnedModel.TreatmentsPrices.List(r => r.treatments_id == treatments_id && r.treatmentspriceslists_id == Convert.ToInt32(((DGUIGHFUtilsUI.DGComboBoxItem)comboBox_tabTreatmentsPrices_filterPriceslists.SelectedItem).Id)).ToList();
            else
                treatmentspricesl = _dentnedModel.TreatmentsPrices.List(r => r.treatments_id == treatments_id).ToList();
            IEnumerable<VTreatmentsPrices> vTreatmentsPrices =
            treatmentspricesl.Select(
            r => new VTreatmentsPrices
            {
                treatmentsprices_id = r.treatmentsprices_id,
                price = (double)r.treatmentsprices_price,
                pricelist = _dentnedModel.TreatmentsPricesLists.Find(r.treatmentspriceslists_id).treatmentspriceslists_name
            }).ToList();

            ret = DGDataTableUtils.ToDataTable<VTreatmentsPrices>(vTreatmentsPrices);

            return ret;
        }

        /// <summary>
        /// Tab Datagrid filter handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advancedDataGridView_tabTreatmentsPrices_list_FilterStringChanged(object sender, EventArgs e)
        {
            vTreatmentsPricesBindingSource.Filter = advancedDataGridView_tabTreatmentsPrices_list.FilterString;
        }

        /// <summary>
        /// Tab Datagrid sort handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advancedDataGridView_tabTreatmentsPrices_list_SortStringChanged(object sender, EventArgs e)
        {
            vTreatmentsPricesBindingSource.Sort = advancedDataGridView_tabTreatmentsPrices_list.SortString;
        }

        /// <summary>
        /// Load the tab DataSource
        /// </summary>
        /// <returns></returns>
        private object GetDataSourceEdit_tabTreatmentsPrices()
        {
            return DGUIGHFData.LoadEntityFromCurrentBindingSource<treatmentsprices, DentneDModel>(_dentnedModel.TreatmentsPrices, vTreatmentsPricesBindingSource, new string[] { "treatmentsprices_id" });
        }

        /// <summary>
        /// Do actions after Save
        /// </summary>
        /// <param name="item"></param>
        private void AfterSaveAction_tabTreatmentsPrices(object item)
        {
            DGUIGHFData.SetBindingSourcePosition<treatmentsprices, DentneDModel>(_dentnedModel.TreatmentsPrices, item, vTreatmentsPricesBindingSource);
        }
        
        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item"></param>
        private void Add_tabTreatmentsPrices(object item)
        {
            DGUIGHFData.Add<treatmentsprices, DentneDModel>(_dentnedModel.TreatmentsPrices, item);
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="item"></param>
        private void Update_tabTreatmentsPrices(object item)
        {
            DGUIGHFData.Update<treatmentsprices, DentneDModel>(_dentnedModel.TreatmentsPrices, item);
        }

        /// <summary>
        /// Remove an item
        /// </summary>
        /// <param name="item"></param>
        private void Remove_tabTreatmentsPrices(object item)
        {
            DGUIGHFData.Remove<treatmentsprices, DentneDModel>(_dentnedModel.TreatmentsPrices, item);
        }

        /// <summary>
        /// New tab button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_tabTreatmentsPrices_new_Click(object sender, EventArgs e)
        {
            if (vTreatmentsBindingSource.Current != null)
            {
                if (AddClick(tabElement_tabTreatmentsPrices))
                {
                    ((treatmentsprices)treatmentspricesBindingSource.Current).treatments_id = (((DataRowView)vTreatmentsBindingSource.Current).Row).Field<int>("treatments_id");
                    if (comboBox_tabTreatmentsPrices_filterPriceslists.SelectedIndex != -1 && comboBox_tabTreatmentsPrices_filterPriceslists.SelectedIndex != 0)
                    {
                        ((treatmentsprices)treatmentspricesBindingSource.Current).treatmentspriceslists_id = Convert.ToInt32(((DGUIGHFUtilsUI.DGComboBoxItem)comboBox_tabTreatmentsPrices_filterPriceslists.SelectedItem).Id);
                        treatmentspriceslists_idComboBox.Enabled = false;
                    }
                    treatmentspricesBindingSource.ResetBindings(true);
                }
            }
        }

        /// <summary>
        /// Treatments prices changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_tabTreatmentsPrices_filterPriceslists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsBindingSourceLoading)
                return;

            ReloadTab(tabElement_tabTreatmentsPrices);
        }

        /// <summary>
        /// Treatments lists changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treatmentspriceslists_idComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsBindingSourceLoading)
                return;

            if(treatmentspriceslists_idComboBox.SelectedIndex != -1 && (tabElement_tabTreatmentsPrices.CurrentEditingMode == EditingMode.C || tabElement_tabTreatmentsPrices.CurrentEditingMode == EditingMode.U))
            {
                int treatments_id = -1;
                if (vTreatmentsBindingSource.Current != null)
                {
                    treatments_id = (((DataRowView)vTreatmentsBindingSource.Current).Row).Field<int>("treatments_id");
                }
                if (treatments_id != -1)
                {
                    treatments treatments = _dentnedModel.Treatments.Find(treatments_id);
                    treatmentspriceslists treatmentspriceslist = _dentnedModel.TreatmentsPricesLists.Find(treatmentspriceslists_idComboBox.SelectedValue);
                    if (treatments != null && treatmentspriceslist != null)
                    {
                        ((treatmentsprices)treatmentspricesBindingSource.Current).treatmentspriceslists_id = treatmentspriceslist.treatmentspriceslists_id;
                        ((treatmentsprices)treatmentspricesBindingSource.Current).treatmentsprices_price = Math.Round(treatments.treatments_price * treatmentspriceslist.treatmentspriceslists_multiplier, 2);
                    }
                    treatmentspricesBindingSource.ResetBindings(true);
                }
            }
        }

        /// <summary>
        /// Combobox autocomplete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treatmentspriceslists_idComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DGUIGHFUtilsUI.DGComboBoxAutoComplete.OnKeyPress((ComboBox)sender, e);
        }

        #endregion


    }
}