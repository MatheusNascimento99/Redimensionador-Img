using System.Drawing;


Console.WriteLine("Iniciando Redimensionador!");

Thread thread = new Thread(Redimensionar);
thread.Start();

static void Redimensionar()


{
    #region "Diretórios"
    string diretorioEntrada = "arquivo_entrada";
    string diretorioRedimen = "arquivo_redimensionado";
    string diretorioFinalizado = "arquivo_finalizado";


    if (!Directory.Exists(diretorioEntrada)) {
        Directory.CreateDirectory(diretorioEntrada);

    }

    if (!Directory.Exists(diretorioRedimen)){
        Directory.CreateDirectory(diretorioRedimen);
    }

    if (!Directory.Exists(diretorioFinalizado)) {
        Directory.CreateDirectory(diretorioFinalizado);
    }
    #endregion


    FileStream fileStream;
    FileInfo fileInfo;

    while (true) 
    {
        //programa verifica se há arquivo na pasta de entrada

        var arquivosEntrada = Directory.EnumerateFiles(diretorioEntrada);
        //então ele le o tamanho do arquivo que irá redimensionar
        int novaAltura = 200;

        foreach (var arquivo in arquivosEntrada)
        {

            fileStream  = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);         //le o arquivo de várias formas
            fileInfo = new FileInfo(arquivo);

            string caminho = Environment.CurrentDirectory + @"\" + diretorioRedimen
                + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name; // defini o caminho a ser salvo
           

            //executa o redimensionamento + copia os arquivos redimensionados para a pasta REDIMENSIONADOS
            Redimensionador(Image.FromStream(fileStream), novaAltura, diretorioRedimen);

            //fecha o arquivo
            fileStream.Close();

            //então MOVE o arquivo da pasta de entrada para a pasta de FINALIZDOS

            string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorioFinalizado + @"\" + fileInfo.Name;
            fileInfo.MoveTo(diretorioFinalizado);
        }
        Thread.Sleep(new TimeSpan(0, 0, 2));

    }



    /// <summary>
    /// imagem --> imagem a ser redimensionada
    /// altura --> altura que desejamos redimensionar
    /// caminho --> caminho a onde iremos gravar o arqiuvo ja redimensionado
    /// </summary>

    static void Redimensionador(Image imagem, int altura, string caminho)
    {
        double ration = (double)altura / imagem.Height;

        int novaLargura = (int)(imagem.Width * ration);
        int novaAlturara = (int)(imagem.Height * ration);

        Bitmap novaImage = new Bitmap(novaLargura, novaAlturara);

        using (Graphics g = Graphics.FromImage(novaImage))
        {
            g.DrawImage(imagem, 0, 0, novaLargura, novaAlturara);
        }


        novaImage.Save(caminho);
        imagem.Dispose(); // tira da memória para não ocupar mais 


    }



}





