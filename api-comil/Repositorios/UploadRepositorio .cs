using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Http;

namespace api_comil.Repositorios
{
    public class UploadRepositorio 
    {
         public string Upload (IFormFile arquivo, string pasta, string local ) {

            var pathToSave = Path.Combine (Directory.GetCurrentDirectory (), pasta+'/'+local);

            if (arquivo.Length > 0) {
                var fileName = ContentDispositionHeaderValue.Parse (arquivo.ContentDisposition).FileName.Trim ('"');
                var fullPath = Path.Combine (pathToSave, fileName);

                using (var stream = new FileStream (fullPath, FileMode.Create)) {
                    arquivo.CopyTo (stream);
                }                    

                return local + "/" +fileName;
            } else {
                return null;    
            }           
        }

    }
}