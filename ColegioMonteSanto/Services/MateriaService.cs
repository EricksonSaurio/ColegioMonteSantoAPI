using ColegioMonteSanto.Models;
using Microsoft.EntityFrameworkCore;
using ColegioMonteSanto.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MateriaService : IMateriaService
{
    private readonly ColegioMonteSantoContext _context;

    public MateriaService(ColegioMonteSantoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MateriaModel>> GetMateriasByProfesorIdAsync(int profesorId)
    {
        return await _context.Materias
            .Where(m => m.profesorid == profesorId)
            .ToListAsync();
    }

    public async Task<MateriaModel> CreateMateriaAsync(MateriaModel model)
    {
        _context.Materias.Add(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<bool> UpdateMateriaAsync(int id, MateriaModel model)
    {
        var materia = await _context.Materias.FindAsync(id);
        if (materia == null) return false;

       
        materia.nombre_materia = model.nombre_materia;
        materia.estado = model.estado;
        materia.profesorid = model.profesorid;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMateriaAsync(int id)
    {
        var materia = await _context.Materias.FindAsync(id);
        if (materia == null) return false;

        _context.Materias.Remove(materia);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<MateriaModel> GetMateriaByIdAsync(int id)
    {
        return await _context.Materias.FindAsync(id);
    }
}
