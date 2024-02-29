namespace Scarpe.Models
{
    public static class StaticDb
    {
        private static int _maxId = 3;
        private static List<Shoe> _shoes = [
            new Shoe(){Id = 1, Nome = "Trota", Prezzo = 1500, Descrizione = "Bellissima scarpa"},
            new Shoe(){Id = 2, Nome = "Salmone", Prezzo = 3000, Descrizione = "Bellissima scarpa"},
            new Shoe(){Id = 3, Nome = "Sgombro", Prezzo = 800, Descrizione = "Bellissima scarpa"},
        ];

        public static List<Shoe> GetAll()
        {
            List<Shoe> notDeletedShoes = [];
            foreach(var shoe in _shoes)
            {
                if (shoe.DeletedAt is null)
                {
                    notDeletedShoes.Add(shoe);
                }
            }
            return notDeletedShoes;

            //return _shoes;
        }

        public static List<Shoe> GetAllDeleted()
        {
            List<Shoe> deletedShoes = [];
            foreach (var shoe in _shoes)
            {
                if (shoe.DeletedAt is not null)
                {
                    deletedShoes.Add(shoe);
                }
            }
            return deletedShoes;

            //return _shoes;
        }

        public static Shoe? Recover(int idToRecover)
        {
            int? index = findShoeIndex(idToRecover);

            if (index is not null)
            {
                var shoeRecovered = _shoes[(int)index];
                shoeRecovered.DeletedAt = null;
                return shoeRecovered;
            }

            return null;
        }

        public static Shoe? GetById(int? id)
        {
            if (id is null) return null;

            for(int i = 0; i < _shoes.Count; i++)
            {
                Shoe shoe = _shoes[i];
                if (shoe.Id == id)
                {
                    return shoe;
                }
            }

            return null;
        }

        public static Shoe Add(Shoe shoe)
        {
            _maxId++;
            var newShoe = new Shoe() { Id = _maxId, Nome = shoe.Nome, Prezzo = shoe.Prezzo, Descrizione = shoe.Descrizione };
            _shoes.Add(newShoe);
            return newShoe;
        }

        public static Shoe? Modify(Shoe shoe)
        {
            foreach(var shoeInList in _shoes)
            {
                if (shoeInList.Id == shoe.Id)
                {
                    shoeInList.Nome = shoe.Nome;
                    shoeInList.Prezzo = shoe.Prezzo;
                    shoeInList.Descrizione = shoe.Descrizione;
                    return shoeInList;
                }
            }
            return null;
        }

        public static Shoe? SoftDelete(int idToDelete)
        {
            // ha bisogno di un ulteriore campo nella tabella DeletedAt (o null o data di eliminazione)
            int? deletedIndex = findShoeIndex(idToDelete);

            if (deletedIndex is not null)
            {
                var shoeDeleted = _shoes[(int)deletedIndex];
                shoeDeleted.DeletedAt = DateTime.UtcNow;
                return shoeDeleted;
            }

            return null;
        }

        public static Shoe? HardDelete(int idToDelete)
        {
            // elimina per sempre il dato

            int? deletedIndex = findShoeIndex(idToDelete);
            

            if (deletedIndex is not null)
            {
                var shoeDeleted = _shoes[(int)deletedIndex];
                _shoes.RemoveAt((int)deletedIndex);
                return shoeDeleted;
            }

            return null;
        }

        private static int? findShoeIndex(int idToDelete)
        {
            int i;
            bool shoeFound = false;
            for (i = 0; i < _shoes.Count; i++)
            {
                if (_shoes[i].Id == idToDelete)
                {
                    shoeFound = true;
                    break;
                }
            }

            if (shoeFound) return i;
            return null;
        }
    }
}
