# WPFDoodleBoard
WPF 涂鸦、橡皮擦、矩形、圆、箭头、清除、撤销、恢复
    立方体、圆柱体、圆锥体（支持图形平移、拉伸、旋转）
    
## DoodleEnumType 支持的形状
```
public enum DoodleEnumType
{
    eraser = 0, //橡皮擦
    draw,       //涂鸦
    line,       //直线
    rect,       //矩形
    circle,     //圆
    arrow,      //箭头
    cube,       //立方体
    cylinder,   //圆柱体
    cone        //圆锥体
}

```
  
## DoodleControl 使用例子
```
//设置涂鸦类型
doodle.SetDrawType(Doodle.DoodleEnum.DoodleEnumType.draw);
//撤销
doodle.Undo();
//清空
doodle.Clear();


//涂鸦模式
doodle.SetCanDraw();

//选择模式（点击形状可以平移、拉伸、旋转）
doodle.SetCanSelect();

```

## 截图
![Alt text](https://github.com/JR-Dun/WPFDoodleBoard/raw/master/Screenshots/1.png)
![Alt text](https://github.com/JR-Dun/WPFDoodleBoard/raw/master/Screenshots/2.png)

