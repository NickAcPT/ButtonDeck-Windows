﻿using ButtonDeck.Forms.FirstSetup;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms
{
    public partial class FirstSetupForm : TemplateForm
    {
        public bool FinishedSetup { get; set; }
        private int currentPage;
        private PageTemplate currentPageTemplate;
        private List<PageTemplate> setupPages = new List<PageTemplate>() {
            new IntroPage(),
            new ThemeSelectionPage(),
            new DeviceNamePage()
        };

        public FirstSetupForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ChangePage(currentPage);
        }


        public void ChangePage(int pageNumber)
        {
            if (currentPageTemplate != null && !currentPageTemplate.CanProgress) return;
            if (pageNumber < setupPages.Count) {
                PageTemplate page = setupPages[pageNumber];
                if (page != null) {
                    if (currentPageTemplate != null) currentPageTemplate.SaveProgress();
                    panel1.Controls.Clear();
                    page.Dock = DockStyle.Fill;
                    page.ForeColor = ForeColor;
                    panel1.Controls.Add(page);
                    currentPageTemplate = page;
                    currentPage = pageNumber;
                }
            }
            label1.Text = string.Format(label1.Tag.ToString(), currentPage + 1, setupPages.Count);
            if (currentPage == setupPages.Count - 1)
                modernButton1.Text = "Finish";


        }

        private void ModernButton1_Click(object sender, EventArgs e)
        {

            if (currentPage < setupPages.Count - 1) {
                ChangePage(++currentPage);
                ModifyColorScheme(Controls.OfType<Control>());
            } else {
                if (currentPageTemplate != null && !currentPageTemplate.CanProgress) return;
                if (currentPageTemplate != null) currentPageTemplate.SaveProgress();
                FinishedSetup = true;
                Close();
            }
        }
    }
}
