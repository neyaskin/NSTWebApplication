﻿Hall of Fame

1. При удалении Person, теперь так же удаляются связанные с ним Skills. До этого не работало из-за того что не прописал связь в Models.Skills.cs;
2. Сделал чтобы сервис возвращал исключение при не нахождении объектов. Контроллер тоже возвращает ответ NotFound;
3. Добавил комментарии к методам