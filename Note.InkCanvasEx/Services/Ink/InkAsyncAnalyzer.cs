﻿using System;
using Windows.UI.Xaml;
using Windows.Foundation;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.Events;
using System.Collections.Generic;
using Windows.UI.Input.Inking.Analysis;

namespace Note.InkCanvasEx.Services.Ink
{
    //笔迹识别逻辑
    public class InkAsyncAnalyzer
    {
        private const double IdleWaitingTime = 400;
        private readonly InkCanvas _inkCanvas;
        private readonly InkStrokesService _strokesService;
        private readonly DispatcherTimer dispatcherTimer;
        public InkAsyncAnalyzer(InkCanvas inkCanvas, InkStrokesService strokesService)
        {
            _inkCanvas = inkCanvas;
            _inkCanvas.InkPresenter.StrokeInput.StrokeStarted += (s, e) => StopTimer();
            _inkCanvas.InkPresenter.StrokesErased += (s, e) => RemoveStrokes(e.Strokes);
            _inkCanvas.InkPresenter.StrokesCollected += (s, e) => AddStrokes(e.Strokes);

            _strokesService = strokesService;
            _strokesService.AddStrokeEvent += StrokesService_AddStrokeEvent;
            _strokesService.RemoveStrokeEvent += StrokesService_RemoveStrokeEvent;
            _strokesService.MoveStrokesEvent += StrokesService_MoveStrokesEvent;
            _strokesService.CutStrokesEvent += StrokesService_CutStrokesEvent;
            _strokesService.PasteStrokesEvent += StrokesService_PasteStrokesEvent;
            _strokesService.ClearStrokesEvent += StrokesService_ClearStrokesEvent;
            _strokesService.LoadInkFileEvent += StrokesService_LoadInkFileEvent;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(IdleWaitingTime);
        }
        public InkAnalyzer InkAnalyzer { get; private set; } = new InkAnalyzer();
        public bool IsAnalyzing => InkAnalyzer.IsAnalyzing;
        public async Task<bool> AnalyzeAsync(bool clean = false)
        {
            StopTimer();
            if (IsAnalyzing)
            {
                StartTimer();
                return false;
            }
            if (clean == true)
            {
                InkAnalyzer.ClearDataForAllStrokes();
                InkAnalyzer.AddDataForStrokes(_strokesService.GetStrokes());
            }
            var result = await InkAnalyzer.AnalyzeAsync();
            return result.Status == InkAnalysisStatus.Updated;
        }
        public IInkAnalysisNode FindHitNode(Point position)
        {
            var node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkWord);
            if (node == null)
            {
                node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkBullet);
                if (node == null)
                {
                    node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkDrawing);
                }
            }
            return node;
        }
        public void AddStroke(InkStroke stroke)
        {
            StopTimer();
            InkAnalyzer.AddDataForStroke(stroke);
            StartTimer();
        }
        public void AddStrokes(IReadOnlyList<InkStroke> strokes)
        {
            StopTimer();
            InkAnalyzer.AddDataForStrokes(strokes);
            StartTimer();
        }
        public void RemoveStroke(InkStroke stroke)
        {
            StopTimer();
            InkAnalyzer.RemoveDataForStroke(stroke.Id);
            StartTimer();
        }
        public void RemoveStrokes(IReadOnlyList<InkStroke> strokes)
        {
            StopTimer();
            foreach (var stroke in strokes)
            {
                InkAnalyzer.RemoveDataForStroke(stroke.Id);
            }
            StartTimer();
        }
        public void ReplaceStroke(InkStroke stroke)
        {
            InkAnalyzer.ReplaceDataForStroke(stroke);
        }
        public void ClearAnalysis()
        {
            StopTimer();
            InkAnalyzer.ClearDataForAllStrokes();
        }
        public void StartTimer() => dispatcherTimer.Start();
        public void StopTimer() => dispatcherTimer.Stop();
        private IInkAnalysisNode FindHitNodeByKind(Point position, InkAnalysisNodeKind kind)
        {
            var nodes = InkAnalyzer.AnalysisRoot.FindNodes(kind);
            foreach (var node in nodes)
            {
                if (RectHelper.Contains(node.BoundingRect, position))
                {
                    return node;
                }
            }
            return null;
        }
        private void StrokesService_AddStrokeEvent(object sender, AddStrokeEventArgs e) => AddStroke(e.NewStroke);
        private void StrokesService_RemoveStrokeEvent(object sender, RemoveStrokeEventArgs e) => RemoveStroke(e.RemovedStroke);
        private void StrokesService_ClearStrokesEvent(object sender, EventArgs e) => ClearAnalysis();
        private async void StrokesService_MoveStrokesEvent(object sender, MoveStrokesEventArgs e)
        {
            foreach (var stroke in e.Strokes)
            {
                ReplaceStroke(stroke);
            }
            await AnalyzeAsync(true);
        }
        private void StrokesService_PasteStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            foreach (var stroke in e.Strokes)
            {
                AddStroke(stroke);
            }
        }
        private void StrokesService_CutStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            foreach (var stroke in e.Strokes)
            {
                RemoveStroke(stroke);
            }
        }
        private async void StrokesService_LoadInkFileEvent(object sender, EventArgs e) => await AnalyzeAsync(true);
        private async void DispatcherTimer_Tick(object sender, object e) => await AnalyzeAsync();
    }
}
