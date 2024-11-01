using ColegioMonteSanto.Models;

public interface IMateriaService
{
    Task<IEnumerable<MateriaModel>> GetMateriasByProfesorIdAsync(int profesorId);
    Task<MateriaModel> CreateMateriaAsync(MateriaModel model);
    Task<bool> UpdateMateriaAsync(int id, MateriaModel model);
    Task<bool> DeleteMateriaAsync(int id);
    Task<MateriaModel> GetMateriaByIdAsync(int id);
}
