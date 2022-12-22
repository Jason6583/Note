using System.ComponentModel;

namespace Note.InkCanvasEx.SDK
{
    /// <summary>识别手势类型</summary>
    public enum GestureType
    {
        /// <summary>未知</summary>
        [Description("未知")] GESTURE_TYPE_UNKNOWN,
        /// <summary>擦除</summary>
        [Description("擦除")] GESTURE_TYPE_DELETE,
        /// <summary>圆（框选）</summary>
        [Description("圆（框选）")] GESTURE_TYPE_CIRCLE_SELECT,
        /// <summary>插入空格（向下直线）</summary>
        [Description("插入空格（向下直线）")] GESTURE_TYPE_DOWN_LINE_INSERT_SPACE,
        /// <summary>删除空格（向上直线）</summary>
        [Description("删除空格（向上直线）")] GESTURE_TYPE_UP_LINE_DELETE_SPACE,
        /// <summary>插入文字（开口向上折线）</summary>
        [Description("插入文字（开口向上折线）")] GESTURE_TYPE_UP_POLYLINE_INSERT_WORD,
        /// <summary>插入文字（开口向下折线）</summary>
        [Description("插入文字（开口向下折线）")] GESTURE_TYPE_DOWN_POLYLINE_INSERT_WORD,
        /// <summary>回车功能（90度横折线）</summary>
        [Description("回车功能（90度横折线）")] GESTURE_TYPE_CROSS_BREAK_ENTER,
        /// <summary>Tab功能键（横线加箭头）</summary>
        [Description("Tab功能键（横线加箭头）")] GESTURE_TYPE_LINE_ARROW_TAB,
        /// <summary>矩形（框选）</summary>
        [Description("矩形（框选）")] GESTURE_TYPE_RECTANGLE_SELECT,
        /// <summary>曲线（选中）</summary>
        [Description("曲线（选中）")] GESTURE_TYPE_CURVE_SELECTED,
        /// <summary>横线（删除）</summary>
        [Description("横线（删除）")] GESTURE_TYPE_LINE_DELETE,
        /// <summary>同心圆</summary>
        [Description("同心圆")] GESTURE_TYPE_CONCENTRIC_CIRCLES,
        /// <summary>其他</summary>
        [Description("其他")] GESTRUE_TYPE_OTHER,
    }
}
