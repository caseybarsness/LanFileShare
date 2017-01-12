using System;
using WebApplication.Models;
public class ShareFile{

    public int ShareFileID { get; set; }
    public string FileName { get; set; }
    public byte[] File { get; set; }
    public DateTime CreatedDate { get; set; }
    public string PublicIpAddress { get; set; }
    public virtual ApplicationUser Person { get; set; }
    
}