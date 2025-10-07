using ABCinema_WPF.Data.Repositories;

namespace ABCinema_WPF.Models;

public static class HallModel
{
    public static async Task<List<HallItem>> GetHallItemsAsync()
    {
        var list = await HallRepository.GetAll();
        
        return list.Select(hall => new HallItem(
            hall.Id,
            hall.Name,
            hall.Rows,
            hall.SeatsPerRow,
            hall.Rows * hall.SeatsPerRow
        )).ToList();
    }


    public static async Task AddHallAsync(Hall hall)
        => await HallRepository.AddHall(hall);
    
    public static int GetCapacity(this Hall hall)
        => hall.Rows * hall.SeatsPerRow;
    
    public static async Task UpdateHallAsync(Hall hall)
        => await HallRepository.Update(hall);

}