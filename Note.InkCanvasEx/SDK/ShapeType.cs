using System.ComponentModel;

namespace Note.InkCanvasEx.SDK
{
    /// <summary>识别形状类型</summary>
    public enum ShapeType
    {
        /// <summary>未知</summary>
        [Description("未知")] SHAPE_TYPE_UNKNOWN = 0,
        /// <summary>直线</summary>
        [Description("直线")] SHAPE_TYPE_LINE = 1,
        /// <summary>圆</summary>
        [Description("圆")] SHAPE_TYPE_CIRCLE = 2,
        /// <summary>折线</summary>
        [Description("折线")] SHAPE_TYPE_POLYLINE = 3,
        /// <summary>普通曲线</summary>
        SHAPE_TYPE_CURVE = 5,
        /// <summary>矩形</summary>
        [Description("矩形")] SHAPE_TYPE_RECTANGLE = 6,
        /// <summary>平行四边形</summary>
        [Description("平行四边形")] SHAPE_TYPE_PARALLELOGRAM = 7,
        /// <summary>梯形（已屏蔽）</summary>
        [Description("梯形")] SHAPE_TYPE_TRAPEZOID = 8,
        /// <summary>菱形</summary>
        [Description("菱形")] SHAPE_TYPE_DIAMOND = 9,
        /// <summary>等腰三角形</summary>
        [Description("等腰三角形")] SHAPE_TYPE_ISOSCELES_TRIANGLE = 10, // 0x0000000A
        /// <summary>等边三角形</summary>
        [Description("等边三角形")] SHAPE_TYPE_EQUILATERAL_TRIANGLE = 11, // 0x0000000B
        /// <summary>五角星</summary>
        [Description("五角星")] SHAPE_TYPE_PENTAGRAM = 12, // 0x0000000C
        /// <summary>五边形</summary>
        [Description("五边形")] SHAPE_TYPE_PENTAGON = 13, // 0x0000000D
        /// <summary>抛物线</summary>
        [Description("抛物线")] SHAPE_TYPE_PARABOLA = 14, // 0x0000000E
        /// <summary>直线起点带箭头</summary>
        [Description("直线起点带箭头")] SHAPE_TYPE_LINE_START_ARROW = 15, // 0x0000000F
        /// <summary>直线终点带箭头</summary>
        [Description("直线终点带箭头")] SHAPE_TYPE_LINE_END_ARROW = 16, // 0x00000010
        /// <summary>直线两端都带箭头</summary>
        [Description("直线两端都带箭头")] SHAPE_TYPE_LINE_STARTEND_ARROW = 17, // 0x00000011
        /// <summary>抛物线起点带箭头</summary>
        [Description("抛物线起点带箭头")] SHAPE_TYPE_PARABOLA_START_ARROW = 18, // 0x00000012
        /// <summary>抛物线终点带箭头</summary>
        [Description("抛物线终点带箭头")] SHAPE_TYPE_PARABOLA_END_ARROW = 19, // 0x00000013
        /// <summary>抛物线两端都带箭头</summary>
        [Description("抛物线两端都带箭头")] SHAPE_TYPE_PARABOLA_STARTEND_ARROW = 20, // 0x00000014
        /// <summary>心形</summary>
        [Description("心形")] SHAPE_TYPE_HEART = 21, // 0x00000015
        /// <summary>云朵</summary>
        [Description("云朵")] SHAPE_TYPE_CLOUD = 22, // 0x00000016
        /// <summary>普通四边形</summary>
        [Description("普通四边形")] SHAPE_TYPE_QUADRILATERAL = 23, // 0x00000017
        /// <summary>普通三角形</summary>
        [Description("普通三角形")] SHAPE_TYPE_TRIANGLE = 24, // 0x00000018
        /// <summary>六边形</summary>
        [Description("六边形")] SHAPE_TYPE_HEXAGON = 25, // 0x00000019
    }
}
