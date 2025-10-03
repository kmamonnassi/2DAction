using UnityEngine;

public static class UnityExtension
{
    //Spriteと同じサイズ(部分)のTextureを取得する
    public static Texture2D GetTextureSameSizeAsSprite(this Sprite sprite)
    {
        //Spriteから全体のテクスチャを取得
        Texture2D texture = sprite.texture;

        //スプライトの位置とサイズを取得
        int x = (int)sprite.textureRect.x, y = (int)sprite.textureRect.y;
        int width = (int)sprite.textureRect.width, height = (int)sprite.textureRect.height;

        //スプライト部分の画素値だけ抜き出す
        Color[] pixels = texture.GetPixels(x, y, width, height);

        //画素値を使ってSprite部分だけのテクスチャを作成
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        return newTexture;
    }
}