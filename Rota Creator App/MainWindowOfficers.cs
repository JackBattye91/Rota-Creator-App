using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public partial class MainWindow
    {
        List<Officer> Officers = new List<Officer>();
        void LoadOfficers()
        {
            FirestoreDb db = FirestoreDb.Create("rotacreator-d84f6");
            CollctionReference = collection = db.Collection("officers");
            
            QuerySnapshot allOfficers = await collection.GetSnapshotAsync();
            foreach(DocumentSnapshot document in allOfficers.Documents)
            {
                Officer officer = document.ConvertTo<Officer>();
                Officers.Add(officer);
            }
        }

        void CreateOfficer(Officer officer)
        {
            Officers.Add(officer);
            
            FirestoreDb db = FirestoreDb.Create("rotacreator-d84f6");
            CollctionReference = collection = db.Collection("officers");

            DocumentReference document = await collection.AddAsync(officer);
        }

        void UpdateOfficer(Officer officer)
        {
            FirestoreDb db = FirestoreDb.Create("rotacreator-d84f6");
            CollctionReference = collection = db.Collection("officers");

            Query query = await collection.WhereEqualTo("id", officer.ID);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach(DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                await documentSnapshot.Reference().SetAsync(officer, SetOptions.MergeAll);
            }
        }

        void DeleteOfficer(Officer officer)
        {
            Officer.Remove(officer);
            FirestoreDb db = FirestoreDb.Create("rotacreator-d84f6");
            CollctionReference = collection = db.Collection("officers");

            Query query= await collection.WhereEqualTo("id", officer.ID);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach(DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                await documentSnapshot.Reference().SetAsync(officer, SetOptions.MergeAll);
            }

            await document.DeleteAsync();
        }
    }
}
