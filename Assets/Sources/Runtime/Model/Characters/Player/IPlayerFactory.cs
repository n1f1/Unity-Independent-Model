namespace Model.Characters.Player
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(PlayerData playerData);
    }
}