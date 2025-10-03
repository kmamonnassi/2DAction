using UnityEngine;

public static class UnityExtension
{
    //Sprite�Ɠ����T�C�Y(����)��Texture���擾����
    public static Texture2D GetTextureSameSizeAsSprite(this Sprite sprite)
    {
        //Sprite����S�̂̃e�N�X�`�����擾
        Texture2D texture = sprite.texture;

        //�X�v���C�g�̈ʒu�ƃT�C�Y���擾
        int x = (int)sprite.textureRect.x, y = (int)sprite.textureRect.y;
        int width = (int)sprite.textureRect.width, height = (int)sprite.textureRect.height;

        //�X�v���C�g�����̉�f�l���������o��
        Color[] pixels = texture.GetPixels(x, y, width, height);

        //��f�l���g����Sprite���������̃e�N�X�`�����쐬
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        return newTexture;
    }
}