namespace Blog.Utils;

public class UtilMensagens
{
    public static string categoria05X03() => $"05X04 - Nenhuma categoria encontrada!";

    public static string categoria05X04(int id) =>
        $"05X04 - Nenhuma categoria encontrada! | Categoria pesquisada - '{id}' |";

    public static string categoria05X05(Exception ex) => $"05X05 - Falha interna no servidor - | {ex.Message} |";
    public static string categoria05XE07(Exception ex) => $"05XE07 - Falha ao deletar a categoria - | {ex.Message} |";

    public static string categoria05XE08(Exception ex) =>
        $"05XE08 - Falha na atualização da categoria - | {ex.Message} |";

    public static string categoria05XE09(Exception ex) =>
        $"05XE09 - Não foi possível incluir a categoria - | {ex.Message} |";

    public static string categoria05XE10(Exception ex) => $"05XE10 - Falha interna no servidor - | {ex.Message} |";
}