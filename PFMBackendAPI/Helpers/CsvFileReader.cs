using System;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Helpers
{
    public class CsvFileReader
    {

        public List<TransactionCsvLine> transactionCsvLines { get; set; }
        public List<CategoryCsvLine> categoryCsvLines { get; set; }

        private const int TransactionCsvFileType = 1;
        private const int CategoryCsvFileType = 2;

        public CsvFileReader()
        {
            transactionCsvLines = new List<TransactionCsvLine>();
            categoryCsvLines = new List<CategoryCsvLine>();
        }


        /// <summary>
        /// Gets a StreamReader for the provided CSV file.
        /// </summary>
        /// <param name="formFile">The CSV file uploaded.</param>
        /// <param name="type">An integer representing the type of CSV data.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value
        /// indicating whether the CSV reader was obtained successfully for the provided type.
        /// </returns>
        public async Task<Boolean> GetCsvReader(IFormFile formFile, int type)
        {

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                stream.Position = 0;
                byte[] bytes = stream.ToArray();

                try
                {
                    using (var fileStream = new FileStream(formFile.FileName, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Close();
                        using (var reader = new StreamReader(fileStream.Name, System.Text.Encoding.UTF8))
                        {
                            var config = new CsvConfiguration(System.Globalization.CultureInfo.CreateSpecificCulture("enUS"))
                            {
                                Delimiter = ",",
                                HasHeaderRecord = true,
                                TrimOptions = TrimOptions.Trim,
                                MissingFieldFound = null,
                                PrepareHeaderForMatch = args => Regex.Replace(args.Header, "-", "").ToLower()
                            };

                            var csv = new CsvReader(reader, config);
                            if (type == TransactionCsvFileType)
                            {
                                transactionCsvLines = csv.GetRecords<TransactionCsvLine>().ToList();
                                return true;
                            }
                            else if (type == CategoryCsvFileType)
                            {
                                categoryCsvLines = csv.GetRecords<CategoryCsvLine>().ToList();
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }
    }

}
