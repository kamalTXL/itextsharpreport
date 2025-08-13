using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMC.CMC.BAL;
using FMC.CMC.Entity;
using System.Globalization;
using FMC.CMC.CommonHelper;
using System.IO;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Ionic.Zip;
using CMC_UI.Reports;
using System.ComponentModel;
using System.Reflection;

namespace CMC_UI.Reports
{
    public class MedicalClaimFormPDFGenerator
    {
        private Document document;
        private PdfWriter writer;
        private BaseFont baseFont;
        private Font headerFont;
        private Font normalFont;
        private Font boldFont;
        private Font smallboldFont;
        private Font smallFont;
        private HttpServerUtility server;

        public MedicalClaimFormPDFGenerator(HttpServerUtility _server)
        {
            server = _server;
            //InitializeFonts();
        }

        //private void InitializeFonts()
        //{
        //    //baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    //headerFont = new Font(baseFont, 14, Font.BOLD);
        //    //normalFont = new Font(baseFont, 10, Font.NORMAL);
        //    //boldFont = new Font(baseFont, 10, Font.BOLD);
        //    //smallboldFont = new Font(baseFont, 7, Font.BOLD);
        //    //smallFont = new Font(baseFont, 8, Font.NORMAL);
        //    // Cache font file path once
            
        //}

        public string GenerateReImbursementClaimForm(DataTable dt_ClaimReImburse)
        {
            MedicalReImbClaimFormData formData = GetFormDataFromRequest(dt_ClaimReImburse);
            //InitializeFonts
            string arialFontPath = server.MapPath("~/fonts/ARIALUNI.TTF");

            // Create fonts once
            BaseFont unicodeBase = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font checkboxFont = new Font(unicodeBase, 16);

            BaseFont baseFont = BaseFont.CreateFont(arialFontPath, BaseFont.CP1252, BaseFont.EMBEDDED);
            Font arialLarge = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);
            Font arialBold = new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK);
            Font arialSmall = new Font(baseFont, 8, Font.NORMAL, BaseColor.BLACK);
            Font arialBoldHeader = new Font(baseFont, 21, Font.BOLD, BaseColor.RED);

            BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font helveticaSmall = new Font(helvetica, 7, Font.NORMAL, BaseColor.BLACK);


            document = new Document(PageSize.A4, 25, 25, 30, 30);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            try
            {
                //PdfPTable headerTable = new PdfPTable(1);
                //headerTable.WidthPercentage = 100;
                //string companyLogo = server.MapPath("~/Images/logo.png");
                //string clientLogo = server.MapPath("~/Images/38.png");
                
                document.Open();

                AddCompanyHeader(document, arialBoldHeader, arialSmall, formData.LogoPath);
                //// Horizontal line
                //DrawHorizontalLine(writer, document);
                // Title
                AddTitle(document, arialBold);
              
                // Instructions
                AddInstructions(document, arialLarge, arialSmall);

                // Member Details Section
                AddMemberInfoSection(document, arialLarge, arialBold);

                //// Clinical Details Section
                //reImburseDocument.Add(new Paragraph("Clinical Details", boldFont));
                //PdfPTable clinicalTable = CreateClinicalDetailsSection(formData);
                //reImburseDocument.Add(clinicalTable);

                ////Signs Details Section
                //reImburseDocument.Add(new Paragraph("Signs & Symptoms:", boldFont));
                //PdfPTable symptomsTable = SignsSymptomsDetailsSection(formData);
                //reImburseDocument.Add(symptomsTable);

                //// Diagnosis
                //AddFormField(reImburseDocument, "Diagnosis:", formData.Diagnosis ?? "");

                //// Management Plan
                //AddFormField(reImburseDocument, "Management plan (Services inside the clinic including injections and investigations):",
                //            formData.ManagementPlan ?? "");
                //reImburseDocument.Add(symptomsTable);
                //// Diagnostic Procedures Section
                //reImburseDocument.Add(new Paragraph("Diagnostic Procedures referred outside If any:", boldFont));
                //PdfPTable diagTable = CreateDiagnosticProceduresSection(formData);
                //reImburseDocument.Add(diagTable);
                //   // Pharmaceuticals Section
                //reImburseDocument.Add(new Paragraph("Pharmaceuticals to be filled by treating Doctor Only (To be filled by the pharmacy)", boldFont));
                //PdfPTable pharmTable = CreatePharmaceuticalsSection(formData);
                //reImburseDocument.Add(pharmTable);

                //// Doctor's Section
                //PdfPTable doctorTable = CreateDoctorSection(formData);
                //reImburseDocument.Add(doctorTable);

                //// Page Footer
                ////CreatePageFooter("Page 1");
                //reImburseDocument.Add(new Paragraph("\n"));
                //Paragraph footer = new Paragraph("FMC/UAE/RB-F/04\n" + "Page 1", smallFont);
                //footer.Alignment = Element.ALIGN_RIGHT;
                //reImburseDocument.Add(footer);

                ////// Generate Page 1
                ////GeneratePage1(formData);

                ////// Add new page for Page 2
                ////reImburseDocument.NewPage();
                ////GeneratePage2(formData);
                //// Bank Details Section
                //reImburseDocument.Add(new Paragraph("Beneficiary Bank Account Details", boldFont));
                //PdfPTable bankTable = CreateBankDetailsSection(formData);
                //reImburseDocument.Add(bankTable);
                //reImburseDocument.Add(new Paragraph("\n"));
                //// Documents Checklist Section
                //reImburseDocument.Add(new Paragraph("List of Documents attached - Checklist", boldFont));
                //PdfPTable checklistTable = CreateDocumentsChecklistSection(formData);
                //reImburseDocument.Add(checklistTable);
                //reImburseDocument.Add(new Paragraph("\n"));
                //// Timeline Section
                //reImburseDocument.Add(new Paragraph("Timeline for Claim submission", boldFont));
                //reImburseDocument.Add(new Paragraph("Service availed within UAE & Outside UAE: As per policy terms and conditions. Kindly refer the Policy Document.", normalFont));
                //reImburseDocument.Add(new Paragraph("Additional Documents submission: Within 3 days of documents request", normalFont));
                //reImburseDocument.Add(new Paragraph("Note: All documents shall be translated in English or Arabic before submission", normalFont));
                //reImburseDocument.Add(new Paragraph("\n"));

                //// Declaration Section
                //reImburseDocument.Add(new Paragraph("Declaration by Claimant:", boldFont));
                //string declaration = "I hereby authorize the physician and healthcare provider to file this claim for medical services on my behalf and I confirm that the above-mentioned examination/Investigation/therapy is given to me by the doctor. I hereby authorize the Physician/Healthcare Provider or any other person who has provided medical services to me to furnish any and all information with regard to medical history, medical condition or medical services and copies of medical records upon request. All the informations pertaining to the claim submission (medical services, reports, investigations, prescriptions, invoices) are related to the treatments adhered.\n\nAlso, confirms that the payment for the eligible services shall be transferred to the bank account furnished above.";
                //reImburseDocument.Add(new Paragraph(declaration, normalFont));
                //reImburseDocument.Add(new Paragraph("\n"));
                //PdfPTable signatureTable = CreateDeclarationSection(formData);
                //reImburseDocument.Add(signatureTable);

                //reImburseDocument.Add(new Paragraph("\n*Beneficiary bank account details/ Company bank account details (If the member not having bank account) - Mandatory.", smallFont));
                //reImburseDocument.Add(new Paragraph("We will not accept salary prepay account. Members having salary prepay account are requested to provide company bank account details with No objection letter from member at the time of claim submission.", smallFont));

                //// Page Footer
                ////CreatePageFooter("Page 2");
                //reImburseDocument.Add(new Paragraph("\n"));
                //Paragraph footer2 = new Paragraph("FMC/UAE/RB-F/04\n" + "Page2", smallFont);
                //footer.Alignment = Element.ALIGN_RIGHT;
                //reImburseDocument.Add(footer2);

                document.Close();
                return Convert.ToBase64String(stream.ToArray());


                }
                catch (Exception ex)
                {
                    //Logging.LogException(ex);
                    //FMCMsgBox.ShowMessage(Page, "Error Occured While downloading report try again later..");
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "PopupMessage", "Ext.net.Mask.hide();", true);
                    return "";
                }
            
        }

        private void DrawHorizontalLine(PdfWriter writer, Document document)
        {
            float y = writer.GetVerticalPosition(false) - 8f;
            PdfContentByte cb = writer.DirectContent;
            cb.SetLineWidth(1f);
            cb.MoveTo(document.LeftMargin, y);
            cb.LineTo(document.PageSize.Width - document.RightMargin, y);
            cb.Stroke();
        }

        private void AddTitle(Document doc, Font arialBold)
        {
            // Create underlined title chunk
            Chunk underlinedTitle = new Chunk("Medical Expenses Reimbursement Claim form", arialBold);
            underlinedTitle.SetUnderline(0.5f, -1.5f);

            // Create a paragraph with center alignment
            Paragraph titleParagraph = new Paragraph(underlinedTitle);
            titleParagraph.Alignment = Element.ALIGN_CENTER;

            // Create a single-column table to hold the title with border
            PdfPTable titleTable = new PdfPTable(1);
            titleTable.WidthPercentage = 100;
            titleTable.SpacingBefore = 5f;
            titleTable.SpacingAfter = 5f;

            // Create a cell with the title paragraph
            PdfPCell titleCell = new PdfPCell();
            titleCell.AddElement(titleParagraph);
            titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
            titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            titleCell.Border = Rectangle.BOX;        // Border on all sides
            titleCell.BorderWidth = 1f;
            titleCell.Padding = 6f;

            // Add the cell to the table
            titleTable.AddCell(titleCell);

            // Add the table to the document
            doc.Add(titleTable);
        }

        private void AddMemberInfoSection(Document document, Font normalFont, Font boldFont)
        {
            // Line 1: Member Name
            PdfPTable line1Table = new PdfPTable(1);
            line1Table.WidthPercentage = 100;

            PdfPCell line1Input = new PdfPCell(new Phrase("1 Member Name ", normalFont));
            line1Input.FixedHeight = 20f;
            line1Input.Padding = 5f;
            line1Input.BorderWidth = 1f;
            line1Input.BorderColor = BaseColor.BLACK;
            line1Table.AddCell(line1Input);

            line1Table.SpacingAfter = 3f;
            document.Add(line1Table);

            // Line 2: Insurance Card Number
            PdfPTable line2Table = new PdfPTable(1);
            line2Table.WidthPercentage = 100;

            PdfPCell line2Input = new PdfPCell(new Phrase("2 Insurance Card Number ", normalFont));
            line2Input.FixedHeight = 20f;
            line2Input.Padding = 5f;
            line2Input.BorderWidth = 1f;
            line2Input.BorderColor = BaseColor.BLACK;
            line2Table.AddCell(line2Input);

            line2Table.SpacingAfter = 3f;
            document.Add(line2Table);

            // Line 3: Insured member Mobile Number
            PdfPTable line3Table = new PdfPTable(1);
            line3Table.WidthPercentage = 100;

            PdfPCell line3Input = new PdfPCell(new Phrase("3 Insured member Mobile Number ", normalFont));
            line3Input.FixedHeight = 20f;
            line3Input.Padding = 5f;
            line3Input.BorderWidth = 1f;
            line3Input.BorderColor = BaseColor.BLACK;
            line3Table.AddCell(line3Input);

            line3Table.SpacingAfter = 3f;
            document.Add(line3Table);

            // Line 4: Empty numbered line
            PdfPTable line4Table = new PdfPTable(1);
            line4Table.WidthPercentage = 100;

            PdfPCell line4Input = new PdfPCell(new Phrase("4 Age/Sex/Nationality ", normalFont));
            line4Input.FixedHeight = 20f;
            line4Input.Padding = 5f;
            line4Input.BorderWidth = 1f;
            line4Input.BorderColor = BaseColor.BLACK;
            line4Table.AddCell(line4Input);

            line4Table.SpacingAfter = 3f;
            document.Add(line4Table);

            // Line 5: Company Name &Employee Number
            PdfPTable line5Table = new PdfPTable(1);
            line5Table.WidthPercentage = 100;

            PdfPCell line5Input = new PdfPCell(new Phrase("5 Company Name &Employee Number ", normalFont));
            line5Input.FixedHeight = 20f;
            line5Input.Padding = 5f;
            line5Input.BorderWidth = 1f;
            line5Input.BorderColor = BaseColor.BLACK;
            line5Table.AddCell(line5Input);

            line5Table.SpacingAfter = 3f;
            document.Add(line5Table);

            // Line 6: Provider Name
            PdfPTable line6Table = new PdfPTable(1);
            line6Table.WidthPercentage = 100;

            PdfPCell line6Input = new PdfPCell(new Phrase("6 Provider Name ", normalFont));
            line6Input.FixedHeight = 20f;
            line6Input.Padding = 5f;
            line6Input.BorderWidth = 1f;
            line6Input.BorderColor = BaseColor.BLACK;
            line6Table.AddCell(line6Input);

            line6Table.SpacingAfter = 3f;
            document.Add(line6Table);

            // Line 7: Company Name &Employee Number
            PdfPTable line7Table = new PdfPTable(1);
            line7Table.WidthPercentage = 100;

            PdfPCell line7Input = new PdfPCell(new Phrase("7 Address & Emirates ", normalFont));
            line7Input.FixedHeight = 20f;
            line7Input.Padding = 5f;
            line7Input.BorderWidth = 1f;
            line7Input.BorderColor = BaseColor.BLACK;
            line7Table.AddCell(line7Input);

            line7Table.SpacingAfter = 3f;
            document.Add(line7Table);

            // Line 8: Date of Visit
            PdfPTable line8Table = new PdfPTable(1);
            line8Table.WidthPercentage = 100;

            PdfPCell line8Input = new PdfPCell(new Phrase("8 Date of Visit ", normalFont));
            line8Input.FixedHeight = 20f;
            line8Input.Padding = 5f;
            line8Input.BorderWidth = 1f;
            line8Input.BorderColor = BaseColor.BLACK;
            line8Table.AddCell(line8Input);

            line8Table.SpacingAfter = 3f;
            document.Add(line8Table);

            //// Mobile Number (Mandatory)
            //PdfPTable mobileTable = new PdfPTable(1);
            //mobileTable.WidthPercentage = 100;

            //Phrase mobilePhrase = new Phrase();
            //mobilePhrase.Add(new Chunk("Insured member Mobile Number ", normalFont));
            //mobilePhrase.Add(new Chunk("Mandatory", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.RED)));

            //PdfPCell mobileInput = new PdfPCell(mobilePhrase);
            //mobileInput.FixedHeight = 25f;
            //mobileInput.Padding = 5f;
            //mobileInput.BorderWidth = 1f;
            //mobileInput.BorderColor = BaseColor.BLACK;
            //mobileTable.AddCell(mobileInput);

            //mobileTable.SpacingAfter = 15f;
            //document.Add(mobileTable);
        }
        //private void AddMemberDetails(Document document, MedicalReImbClaimFormData formData, Font arialLarge, Font arialSmall)
        //{
        //    // Management plan
        //    PdfPTable memberDetailsTable = new PdfPTable(new float[] { 400f });
        //    memberDetailsTable.WidthPercentage = 100;
        //    memberDetailsTable.SpacingBefore = 2f;
        //    //managementTable.DefaultCell.Border = Rectangle.BOX;
        //    memberDetailsTable.DefaultCell.PaddingBottom = 5f;
        //    memberDetailsTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

        //    foreach (var row in formData.ManagementPlan)
        //    {
        //        memberDetailsTable.AddCell(new PdfPCell(new Phrase(row, arialLarge)) { MinimumHeight = 20f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 1f });
        //    }

        //    // Create outer table to hold the single cell
        //    //PdfPTable ManagementDetails = new PdfPTable(1);
        //    //ManagementDetails.WidthPercentage = 100;
        //    //ManagementDetails.AddCell(memberDetailsTable);

        //    // Add to document
        //    document.Add(memberDetailsTable);

        //}

        private void GeneratePage1(MedicalReImbClaimFormData formData)
        {
            // Header Section
            //CreateHeader(formData);

            //// Patient Information Section
            //CreatePatientInfoSection(formData);

            //// Clinical Details Section
            //CreateClinicalDetailsSection(formData);

            //// Diagnostic Procedures Section
            //CreateDiagnosticProceduresSection(formData);

            //// Pharmaceuticals Section
            //CreatePharmaceuticalsSection(formData);

            //// Doctor's Section
            //CreateDoctorSection(formData);

            //// Page Footer
            //CreatePageFooter("Page 1");
        }

        private void GeneratePage2(MedicalReImbClaimFormData formData)
        {
            // Bank Details Section
            //CreateBankDetailsSection(formData);

            //// Documents Checklist Section
            //CreateDocumentsChecklistSection(formData);

            //// Timeline Section
            //CreateTimelineSection();

            //// Declaration Section
            //CreateDeclarationSection(formData);

            //// Page Footer
            //CreatePageFooter("Page 2");
        }

        private MedicalReImbClaimFormData GetFormDataFromRequest(DataTable dt_ClaimReImburse)
        {
            var formData = new MedicalReImbClaimFormData();

            if (dt_ClaimReImburse == null || dt_ClaimReImburse.Rows.Count == 0)
                throw new Exception("Claim ReImbursement form data not found.");

            DataRow row = dt_ClaimReImburse.Rows[0];

            formData.DateOfVisit = DateTime.Now.ToString("dd MMM yyyy");

            formData.MemberName = row["INSURED_PERSON_NAME"] != DBNull.Value
                ? row["INSURED_PERSON_NAME"].ToString()
                : "";

            // Age parsing
            int parsedAge = 0;
            if (row["INSURED_PERSON_AGE"] != DBNull.Value)
            {
                int.TryParse(row["INSURED_PERSON_AGE"].ToString(), out parsedAge);
            }
            formData.Age = parsedAge;

            // Other safe string fields
            formData.Emirates = row["Emirate"] != DBNull.Value ? row["Emirate"].ToString() : "";
            formData.Sex = row["Gender"] != DBNull.Value ? row["Gender"].ToString() : "";
            formData.TelNo = row["CONTACTNO"] != DBNull.Value ? row["CONTACTNO"].ToString() : "";
            formData.MobileNo = row["MobileNo"] != DBNull.Value ? row["MobileNo"].ToString() : "";
            formData.InsuranceCardNumber = row["MemberCardNo"] != DBNull.Value ? row["MemberCardNo"].ToString() : "";
            //formData.ValidUpTo = row["POLICY_CANCEL_DATE"] != DBNull.Value ? row["POLICY_CANCEL_DATE"].ToString() : "";
            formData.CompanyName = row["CustomerName"] != DBNull.Value ? row["CustomerName"].ToString() : "";
            formData.EmployeeNo = row["EmployeeNo"] != DBNull.Value ? row["EmployeeNo"].ToString() : "";
            formData.Nationality = row["Nationality"] != DBNull.Value ? row["Nationality"].ToString() : "";
            formData.LogoPath = row["LogoPath"] != DBNull.Value ? row["LogoPath"].ToString() : "";
            formData.SignatureDate = "";
            // Management plan template
            formData.ManagementPlan = new List<string>
            {
                 { "1. Member Name" },
                 { "2. Insurance Card Number" },
                 { "3. Insured member Mobile Number" },
                 { "4. Age/Sex/Nationality" },
                 { "5. Company Name & Employee Number" },
                 { "6. Provider Name" },
                 { "7. Address & Emirates"},
                 { "8. Date of Visit " },
                 { "                                                            Clinical Details" }
            };

            // Default clinical info
            formData.Temperature = "________";
            formData.BloodPressure = "_________";
            formData.Pulse = "_______";

            return formData;
        }


        public class MedicalReImbClaimFormData
        {
            public string MemberName { get; set; }
            public string InsuranceCardNumber { get; set; }
            public string CompanyName { get; set; }
            public string EmployeeNo { get; set; }
            public string MobileNo { get; set; }
            public string TelNo { get; set; }
            public int Age { get; set; }
            public string Sex { get; set; }
            public string Nationality { get; set; }
            public string RelationWithMember { get; set; }
            public string ProviderName { get; set; }
            public string Address { get; set; }
            public string Emirates { get; set; }
            public string DateOfVisit { get; set; }
            public string DateOfOnsetIllness { get; set; }
            public string TypeOfVisit { get; set; }

            // Clinical Details
            public string Temperature { get; set; }
            public string BloodPressure { get; set; }
            public string Pulse { get; set; }
            public List<string> SignsSymptoms { get; set; } //= new List<string>();
            public string Diagnosis { get; set; }
            public string DiagnosticProcedures { get; set; }
            public List<string> ManagementPlan { get; set; }
            

            // Pharmaceuticals
            public List<PharmaceuticalItem2> Pharmaceuticals { get; set; } //= new List<PharmaceuticalItem2>();

            // Doctor Section
            public string DoctorNameSignature { get; set; }

            // Page 2 - Bank Details
            public string BankName { get; set; }
            public string AccountName { get; set; }
            public string IBANNumber { get; set; }
            public string AccountType { get; set; }
            public string ContactName { get; set; }
            public string ContactNumber { get; set; }
            public string ClaimSubmissionDate { get; set; }

            // Declaration
            public string ClaimantSignature { get; set; }
            public string SignatureDate { get; set; }
            public string LogoPath { get; set; }
        }

        
        public class PharmaceuticalItem2
        {
            public string GenericName { get; set; }
            public string Dose { get; set; }
            public string Duration { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
        }

        private void AddCompanyHeader(Document document, Font arialBoldHeader, Font arialSmall, string clientFileName)
        {
            string imagePath = server.MapPath("~/Images/logo1.png");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
            img.ScaleAbsolute(80f, 80f);
            img.Alignment = Element.ALIGN_LEFT;

            //font spacer
            BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            // Client logo
            string clientImage = "~/ICLogo/" + clientFileName;
            string clientImagePath = server.MapPath(clientImage);
            iTextSharp.text.Image clientImg = iTextSharp.text.Image.GetInstance(clientImagePath);
            clientImg.ScaleAbsolute(80f, 50f);
            clientImg.Alignment = Element.ALIGN_TOP;

            // Create 3-column table
            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 20f, 60f, 20f });

            // Cell 1: Left logo
            PdfPCell leftImageCell = new PdfPCell(img);
            leftImageCell.Border = Rectangle.NO_BORDER;
            leftImageCell.HorizontalAlignment = Element.ALIGN_LEFT;
            leftImageCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            // Cell 2: Center text (all in one line)
            Font blueFont = new Font(arialSmall.BaseFont, arialSmall.Size, Font.NORMAL, BaseColor.BLUE);
            Chunk email = new Chunk("reimbursement@fmchealthcare.ae", blueFont);

            Phrase centerPhrase = new Phrase();
            centerPhrase.Add(new Chunk("F M C NETWORK UAE", arialBoldHeader));
            centerPhrase.Add(new Chunk("\n                 ", arialBoldHeader));
            centerPhrase.Add(new Chunk("\nP. O. BOX: 50430, DUBAI, Tel – 04 3871900, Fax – 04 3977842", arialSmall));
            centerPhrase.Add(new Chunk("\nEmail – ", arialSmall));
            centerPhrase.Add(email);
            centerPhrase.Add(new Chunk(" Helpline Number: 600-565691", arialSmall));

            PdfPCell centerCell = new PdfPCell(centerPhrase);
            centerCell.Border = Rectangle.NO_BORDER;
            centerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            centerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            // Cell 3: Right logo
            PdfPCell rightImageCell = new PdfPCell(clientImg);
            rightImageCell.Border = Rectangle.NO_BORDER;
            rightImageCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rightImageCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            // Add all cells to table
            table.AddCell(leftImageCell);
            table.AddCell(centerCell);
            table.AddCell(rightImageCell);

            document.Add(table);
        }

        private void AddSpacer(Document doc, BaseFont baseFont, float spacing)
        {
            Font smallFont = new Font(baseFont, 3, Font.NORMAL, BaseColor.BLACK);
            Paragraph spacer = new Paragraph(" ", smallFont);
            spacer.SpacingAfter = spacing;
            doc.Add(spacer);
        }
        //private void AddCompanyHeader(Document document, Font arialBoldHeader, Font arialSmall, string clientFileName)
        //{
        //    string imagePath = server.MapPath("~/Images/logo1.png");
        //    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
        //    img.ScaleAbsolute(80f, 80f);
        //    img.Alignment = Element.ALIGN_LEFT;

        //    // Company footer in bordered box
        //    PdfPTable footerTable = new PdfPTable(3);
        //    footerTable.WidthPercentage = 100;

        //    //image cell
        //    PdfPCell imageCell = new PdfPCell(img)
        //    {
        //        Border = Rectangle.NO_BORDER,
        //        VerticalAlignment = Element.ALIGN_LEFT
        //    };

        //    PdfPCell footerCell = new PdfPCell()
        //    {
        //        Border = Rectangle.NO_BORDER,
        //        VerticalAlignment = Element.ALIGN_TOP
        //    };

        //    // Company name
        //    Paragraph companyName = new Paragraph("F M C NETWORK UAE", arialBoldHeader);
        //    companyName.Alignment = Element.ALIGN_CENTER;
        //    companyName.SpacingAfter = 2f;

        //    // Contact details
        //    Paragraph contactDetails = new Paragraph();
        //    contactDetails.Add(new Chunk("P. O. BOX: 50430, DUBAI, Tel – 04 3871900, Fax – 04 3977842", arialSmall));
        //    Font boldFont = new Font(arialSmall.BaseFont, arialSmall.Size, Font.NORMAL, BaseColor.BLUE);
        //    Chunk emailChunk = new Chunk("reimbursement@fmchealthcare.ae", boldFont);

        //    contactDetails.Add(new Chunk("\nEmail – ", arialSmall));
        //    contactDetails.Add(emailChunk);
        //    contactDetails.Add(new Chunk(" Helpline Number: 600-565691", arialSmall));
        //    contactDetails.Alignment = Element.ALIGN_CENTER;

        //    //image of client
        //    string clientImage = "~/ICLogo/" + clientFileName;
        //    string clientImagePath = server.MapPath(clientImage);
        //    iTextSharp.text.Image clientImg = iTextSharp.text.Image.GetInstance(clientImagePath);

        //    clientImg.ScaleAbsolute(80f, 50f);
        //    clientImg.Alignment = Element.ALIGN_TOP;
        //    PdfPCell clientImgCell = new PdfPCell(clientImg)
        //    {
        //        Border = Rectangle.NO_BORDER,
        //        VerticalAlignment = Element.ALIGN_TOP
        //    };

        //    footerCell.AddElement(companyName);
        //    footerCell.AddElement(contactDetails);
        //    footerCell.Border = Rectangle.NO_BORDER;

        //    footerTable.AddCell(imageCell);
        //    footerTable.AddCell(footerCell);
        //    footerTable.AddCell(clientImgCell);
        //    footerTable.SetWidths(new float[] { 20f, 60f, 20f });
        //    document.Add(footerTable);
        //}

        private void AddInstructions(Document doc, Font arialBold, Font arialLarge)
        {
            // Create a table with 1 column
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;

            // Create the paragraph with instructions
            Paragraph instructions = new Paragraph();
            instructions.Add(new Chunk("Instructions to fill the form\n", arialBold));
            instructions.Add(new Chunk("To be filled by the Treating Physician & Claimant\n", arialLarge));
            instructions.Add(new Chunk("All documents must be in English or Arabic", arialLarge));
            instructions.SpacingAfter = 2f;

            // Add paragraph to a cell
            PdfPCell cell = new PdfPCell();
            cell.AddElement(instructions);
            cell.Border = Rectangle.BOX;         // Set border on all sides
            cell.BorderWidth = 1f;               // Adjust thickness
            cell.Padding = 2f;                   // Add padding inside the box
            cell.HorizontalAlignment = Element.ALIGN_LEFT;

            // Add cell to table
            table.AddCell(cell);

            // Add the table to the document
            doc.Add(table);
        }

        private void AddPharmaceuticalsSection(Document document, MedicalReImbClaimFormData formData, Font arialLarge, Font arialSmall)
        {
            // Section header

            // Main table structure
            PdfPTable mainPharmaTable = new PdfPTable(new float[] { 80f, 80f, 80f, 80f, 80f });
            mainPharmaTable.WidthPercentage = 100;
            mainPharmaTable.SpacingAfter = 5f;

            ///Pharmaceuticals header row
            // Headers with gray background
            PdfPCell pharmaHeader = new PdfPCell(new Phrase("Pharmaceuticals (to be filled by treating doctor only)", arialLarge));
            pharmaHeader.HorizontalAlignment = Element.ALIGN_LEFT;
            pharmaHeader.Padding = 5f;
            pharmaHeader.Colspan = 3;
            mainPharmaTable.AddCell(pharmaHeader);

            // Second cell header row for pharmacy section
            PdfPCell pharmacySection = new PdfPCell(new Phrase("(To be filled by the pharmacy)", arialLarge));
            pharmacySection.HorizontalAlignment = Element.ALIGN_CENTER;
            pharmacySection.Padding = 3f;
            pharmacySection.Colspan = 2;
            mainPharmaTable.AddCell(pharmacySection);

            // Headers with gray background
            PdfPCell tradeNameHeader = new PdfPCell(new Phrase("Trade Name", arialLarge));
            tradeNameHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            tradeNameHeader.Padding = 5f;
            mainPharmaTable.AddCell(tradeNameHeader);

            PdfPCell doseHeader = new PdfPCell(new Phrase("Dose", arialLarge));
            doseHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            doseHeader.Padding = 5f;
            mainPharmaTable.AddCell(doseHeader);

            PdfPCell durationHeader = new PdfPCell(new Phrase("Total Duration", arialLarge));
            durationHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            durationHeader.Padding = 5f;
            mainPharmaTable.AddCell(durationHeader);

            PdfPCell quantityHeader = new PdfPCell(new Phrase("Quantity", arialLarge));
            quantityHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            quantityHeader.Padding = 5f;
            mainPharmaTable.AddCell(quantityHeader);

            PdfPCell priceHeader = new PdfPCell(new Phrase("Price", arialLarge));
            priceHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            priceHeader.Padding = 5f;
            mainPharmaTable.AddCell(priceHeader);

            //// Data rows
            //for (int i = 0; i < formData.Pharmaceuticals.Count; i++)
            //{
            //    PharmaceuticalItem item = i < formData.Pharmaceuticals.Count ? formData.Pharmaceuticals[i] : null;

            //    mainPharmaTable.AddCell(new PdfPCell(new Phrase(i + 1 + ")" + (item.TradeName ?? ""), arialLarge)) { MinimumHeight = 20f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
            //    mainPharmaTable.AddCell(new PdfPCell(new Phrase((item.Dose ?? ""), arialLarge)) { MinimumHeight = 20f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
            //    mainPharmaTable.AddCell(new PdfPCell(new Phrase((item.TotalDuration ?? ""), arialLarge)) { MinimumHeight = 20f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
            //    mainPharmaTable.AddCell(new PdfPCell(new Phrase(item.Quantity ?? "", arialLarge)) { MinimumHeight = 20f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
            //    mainPharmaTable.AddCell(new PdfPCell(new Phrase((bool)item.Price.HasValue ? "" : "", arialLarge)) { MinimumHeight = 20f, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3f });
            //}

            // Total row
            decimal totalPrice = 0;// formData.Pharmaceuticals.Where(p => p.Price.HasValue).Sum(p => p.Price.Value);
            PdfPCell exclusionsCell = new PdfPCell(new Phrase("Please apply general exclusions", arialLarge));
            exclusionsCell.Colspan = 3;
            exclusionsCell.HorizontalAlignment = Element.ALIGN_LEFT;
            exclusionsCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            exclusionsCell.Padding = 5f;
            mainPharmaTable.AddCell(exclusionsCell);

            PdfPCell totalLabelCell = new PdfPCell(new Phrase("Total", arialLarge));
            totalLabelCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalLabelCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            totalLabelCell.Padding = 5f;
            mainPharmaTable.AddCell(totalLabelCell);

            PdfPCell totalValueCell = new PdfPCell(new Phrase(totalPrice > 0 ? totalPrice.ToString("0.00") : "", arialLarge));
            totalValueCell.HorizontalAlignment = Element.ALIGN_CENTER;
            totalValueCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            totalValueCell.Padding = 5f;
            mainPharmaTable.AddCell(totalValueCell);

            document.Add(mainPharmaTable);
        }

        private PdfPTable CreatePatientInfoSection(MedicalReImbClaimFormData formData)
        {
            PdfPTable patientTable = new PdfPTable(4);
            patientTable.WidthPercentage = 100;
            patientTable.SetWidths(new float[] { 125f, 25f, 25f, 25f });

            // Row 1
            AddFormField(patientTable, "1. Member Name", formData.MemberName ?? "");
            AddFormField(patientTable, "2. Insurance Card Number", formData.InsuranceCardNumber ?? "");
            AddFormField(patientTable, "3. Company Name & Employee Number", formData.CompanyName + " & " + formData.EmployeeNo ?? "");
            AddFormField(patientTable, "Insured member Mobile Number (Mandatory)", formData.MobileNo ?? "");

            // Row 2
            AddFormField(patientTable, "4. Age/Sex/Nationality", formData.Age + " & " + formData.Sex + " & " + formData.Nationality ?? "");
            AddFormField(patientTable, "5. Relation with Member", formData.RelationWithMember ?? "");
            AddFormField(patientTable, "6. Provider Name", formData.ProviderName ?? "");
            AddFormField(patientTable, "7. Address & Emirates", formData.Address + " & " + formData.Emirates ?? "");

            // Row 3
            AddFormField(patientTable, "8. Date of Visit", formData.DateOfVisit ?? "");
            AddFormField(patientTable, "Date of onset of illness: DD/MM/YYYY", formData.DateOfOnsetIllness ?? "");
            AddFormField(patientTable, "Type of Visit", formData.TypeOfVisit ?? "☐ Emergency ☐ Work Related ☐ New Visit ☐ Follow up visit");
            AddFormField(patientTable, "", "");

            return patientTable;
            
        }

        private PdfPTable CreateClinicalDetailsSection(MedicalReImbClaimFormData formData)
        {
            
            PdfPTable clinicalTable = new PdfPTable(3);
            clinicalTable.WidthPercentage = 100;
            clinicalTable.SetWidths(new float[] { 33f, 33f, 34f });

            AddFormField(clinicalTable, "Temp: ℃", formData.Temperature ?? "");
            AddFormField(clinicalTable, "BP: mmHg", formData.BloodPressure ?? "");
            AddFormField(clinicalTable, "Pulse: /Min", formData.Pulse ?? "");

            return clinicalTable;
        }
        private PdfPTable SignsSymptomsDetailsSection(MedicalReImbClaimFormData formData)
        {
           
            // Signs & Symptoms
            
            PdfPTable symptomsTable = new PdfPTable(1);
            symptomsTable.WidthPercentage = 100;
            formData.SignsSymptoms = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                string symptom = formData.SignsSymptoms.Count >= i ? formData.SignsSymptoms[i - 1] : "";
                AddFormField(symptomsTable, i.ToString(), symptom);
            }
            
            return symptomsTable;
        }

        private PdfPTable CreateDiagnosticProceduresSection(MedicalReImbClaimFormData formData)
        {
            

            PdfPTable diagTable = new PdfPTable(1);
            diagTable.WidthPercentage = 100;

            PdfPCell diagCell = new PdfPCell();
            diagCell.MinimumHeight = 40;
            diagCell.Phrase = new Phrase(formData.DiagnosticProcedures ?? "", normalFont);
            diagTable.AddCell(diagCell);
            return diagTable;
            
        }

        private PdfPTable CreatePharmaceuticalsSection(MedicalReImbClaimFormData formData)
        {
            

            PdfPTable pharmTable = new PdfPTable(6);
            pharmTable.WidthPercentage = 100;
            pharmTable.SetWidths(new float[] { 10f, 25f, 15f, 15f, 15f, 20f });

            // Headers
            AddTableHeader(pharmTable, "Sl.No");
            AddTableHeader(pharmTable, "Generic Name");
            AddTableHeader(pharmTable, "Dose");
            AddTableHeader(pharmTable, "Total Duration");
            AddTableHeader(pharmTable, "Quantity");
            AddTableHeader(pharmTable, "Price");
            formData.Pharmaceuticals = new List<PharmaceuticalItem2>();
            // Rows
            for (int i = 1; i <= 5; i++)
            {
                AddTableCell(pharmTable, i.ToString());
                string genericName = formData.Pharmaceuticals.Count >= i ? formData.Pharmaceuticals[i - 1].GenericName : "";
                string dose = formData.Pharmaceuticals.Count >= i ? formData.Pharmaceuticals[i - 1].Dose : "";
                string duration = formData.Pharmaceuticals.Count >= i ? formData.Pharmaceuticals[i - 1].Duration : "";
                string quantity = formData.Pharmaceuticals.Count >= i ? formData.Pharmaceuticals[i - 1].Quantity : "";
                string price = formData.Pharmaceuticals.Count >= i ? formData.Pharmaceuticals[i - 1].Price : "";

                AddTableCell(pharmTable, genericName);
                AddTableCell(pharmTable, dose);
                AddTableCell(pharmTable, duration);
                AddTableCell(pharmTable, quantity);
                AddTableCell(pharmTable, price);
            }

            return pharmTable;
        }

        private PdfPTable CreateDoctorSection(MedicalReImbClaimFormData formData)
        {
            PdfPTable doctorTable = new PdfPTable(1);
            doctorTable.WidthPercentage = 100;

            PdfPCell doctorCell = new PdfPCell();
            doctorCell.MinimumHeight = 60;
            doctorCell.Phrase = new Phrase("Doctor's Name and signature with seal:\n(Attach prescriptions)\n\n" +
                                         (formData.DoctorNameSignature ?? ""), normalFont);
            doctorTable.AddCell(doctorCell);

            return doctorTable;
        }

        private PdfPTable CreateBankDetailsSection(MedicalReImbClaimFormData formData)
        {
            

            PdfPTable bankTable = new PdfPTable(2);
            bankTable.WidthPercentage = 100;
            bankTable.SetWidths(new float[] { 50f, 50f });

            AddFormField(bankTable, "Bank Name", formData.BankName ?? "");
            AddFormField(bankTable, "Account Name", formData.AccountName ?? "");
            AddFormField(bankTable, "IBAN Number (23 Digits):", formData.IBANNumber ?? "");
            AddFormField(bankTable, "Account Type: ☐ Savings ☐ Current", formData.AccountType ?? "");
            AddFormField(bankTable, "Name", formData.ContactName ?? "");
            AddFormField(bankTable, "Contact No", formData.ContactNumber ?? "");
            AddFormField(bankTable, "Date of Claim Submission", formData.ClaimSubmissionDate ?? "");
            AddFormField(bankTable, "", "");
            return bankTable;
            
        }

        private PdfPTable CreateDocumentsChecklistSection(MedicalReImbClaimFormData formData)
        {
            

            string[] documents = {
                "Original claim form with final diagnosis signed by the Insured and the treating doctor",
                "Original Invoices",
                "Hospital payment receipt with receipt number (Credit card receipt with signature if any)",
                "Discharge summary with summary of diagnosis, treatment in hospital with date of admission and discharge)",
                "Investigation reports",
                "Medicine prescription",
                "Pharmacy Invoices",
                "Police report for all RTA cases",
                "Death Summary (Only in case of death during hospital stay)",
                "Additional Documents"
            };

            PdfPTable checklistTable = new PdfPTable(4);
            checklistTable.WidthPercentage = 100;
            checklistTable.SetWidths(new float[] { 5f, 70f, 12.5f, 12.5f });

            // Headers
            AddTableHeader(checklistTable, "No.");
            AddTableHeader(checklistTable, "Document");
            AddTableHeader(checklistTable, "Yes");
            AddTableHeader(checklistTable, "No");

            for (int i = 0; i < documents.Length; i++)
            {
                AddTableCell(checklistTable, (i + 1).ToString());
                AddTableCell(checklistTable, documents[i]);
                AddTableCell(checklistTable, "☐");
                AddTableCell(checklistTable, "☐");
            }

            return checklistTable;
        }

        //private void CreateTimelineSection()
        //{
        //    reImburseDocument.Add(new Paragraph("Timeline for Claim submission", boldFont));
        //    reImburseDocument.Add(new Paragraph("Service availed within UAE & Outside UAE: As per policy terms and conditions. Kindly refer the Policy Document.", normalFont));
        //    reImburseDocument.Add(new Paragraph("Additional Documents submission: Within 3 days of documents request", normalFont));
        //    reImburseDocument.Add(new Paragraph("Note: All documents shall be translated in English or Arabic before submission", normalFont));
        //    reImburseDocument.Add(new Paragraph("\n"));
        //}

        private PdfPTable CreateDeclarationSection(MedicalReImbClaimFormData formData)
        {
            

            PdfPTable signatureTable = new PdfPTable(2);
            signatureTable.WidthPercentage = 100;
            signatureTable.SetWidths(new float[] { 50f, 50f });

            AddFormField(signatureTable, "Signature of the Claimant:", formData.ClaimantSignature ?? "");
            AddFormField(signatureTable, "Date:", formData.SignatureDate ?? "");

            return signatureTable;
         }

        //private void CreatePageFooter(string pageText)
        //{
        //    reImburseDocument.Add(new Paragraph("\n"));
        //    Paragraph footer = new Paragraph("FMC/UAE/RB-F/04\n" + pageText, smallFont);
        //    footer.Alignment = Element.ALIGN_RIGHT;
        //    reImburseDocument.Add(footer);
        //}

        // Helper Methods
        private void AddFormField(PdfPTable table, string label, string value)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.BOX;
            cell.Padding = 5;
            cell.MinimumHeight = 25;

            Paragraph para = new Paragraph();
            para.Add(new Chunk(label + "\n", boldFont));
            para.Add(new Chunk(value, normalFont));

            cell.AddElement(para);
            table.AddCell(cell);
        }

        private void AddFormField(Document doc, string label, string value)
        {
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.BOX;
            cell.Padding = 5;
            cell.MinimumHeight = 30;

            Paragraph para = new Paragraph();
            para.Add(new Chunk(label + "\n", boldFont));
            para.Add(new Chunk(value, normalFont));

            cell.AddElement(para);
            table.AddCell(cell);

            doc.Add(table);
            doc.Add(new Paragraph("\n"));
        }

        private void AddTableHeader(PdfPTable table, string text)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, boldFont));
            cell.BackgroundColor = new BaseColor(220, 220, 220);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private void AddTableCell(PdfPTable table, string text)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, normalFont));
            cell.Padding = 5;
            cell.MinimumHeight = 25;
            table.AddCell(cell);
        }

    }
    
}