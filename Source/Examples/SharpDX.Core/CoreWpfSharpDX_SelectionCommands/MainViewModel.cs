// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using SharpDX;

namespace FileLoadDemo
{
    using HelixToolkit.SharpDX.Core;
    using HelixToolkit.Wpf.SharpDX;
    using HelixToolkit.SharpDX.Core.Animations;
    using HelixToolkit.SharpDX.Core.Assimp;
    using HelixToolkit.SharpDX.Core.Model;
    using HelixToolkit.SharpDX.Core.Model.Scene;
    using HelixToolkit.Wpf.SharpDX.Controls;
    using Microsoft.Win32;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using ObservableObject = GalaSoft.MvvmLight.ObservableObject;

    public class MainViewModel : ObservableObject
    {
        private string OpenFileFilter = $"{HelixToolkit.SharpDX.Core.Assimp.Importer.SupportedFormatsString}";

        public ICommand OpenFileCommand
        {
            get; set;
        }

        public ICommand ResetCameraCommand
        {
            set; get;
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            private set => Set(ref isLoading, value);
            get => isLoading;
        }

        public Geometry3D PointGeometry { get; } = new PointGeometry3D
        {
            Positions = new Vector3Collection
            {
                new Vector3(10f, 10f, 0f),
                new Vector3(30f,10f,0f)
            }
        };
        public Geometry3D PointGeometry2 { get; } = new PointGeometry3D
        {
            Positions = new Vector3Collection
            {
                new Vector3(10f, 10f, 30f),
                new Vector3(30f,10f,30f)
            }
        };
        public SceneNodeGroupModel3D GroupModel { get; } = new SceneNodeGroupModel3D();

        public EffectsManager EffectsManager { get; }
        public Camera Camera { get; }

        private SynchronizationContext context = SynchronizationContext.Current;
        private HelixToolkitScene scene;
        private List<BoneSkinMeshNode> boneSkinNodes = new List<BoneSkinMeshNode>();
        private List<BoneSkinMeshNode> skeletonNodes = new List<BoneSkinMeshNode>();
        private CompositionTargetEx compositeHelper = new CompositionTargetEx();


        public MainViewModel()
        {
            this.OpenFileCommand = new DelegateCommand(this.OpenFile);
            EffectsManager = new DefaultEffectsManager();
            Camera = new OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(0, -10, -10),
                Position = new System.Windows.Media.Media3D.Point3D(0, 10, 10),
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                FarPlaneDistance = 5000,
                NearPlaneDistance = 0.1f
            };
            ResetCameraCommand = new DelegateCommand(() =>
            {
                (Camera as OrthographicCamera).Reset();
                (Camera as OrthographicCamera).FarPlaneDistance = 5000;
                (Camera as OrthographicCamera).NearPlaneDistance = 0.1f;
            });

            var p1 = new PointNode();
            p1.Geometry = PointGeometry;
            GroupModel.AddNode(p1);

            var p2 = new PointNode();
            p2.Geometry = PointGeometry2;
            GroupModel.AddNode(p1);
            
        }

        private void OpenFile()
        {
            if (isLoading)
            {
                return;
            }
            string path = OpenFileDialog(OpenFileFilter);
            if (path == null)
            {
                return;
            }

            IsLoading = true;
            Task.Run(() =>
            {
                var loader = new Importer();
                return loader.Load(path);
            }).ContinueWith((result) =>
            {
                IsLoading = false;
                if (result.IsCompleted)
                {
                    scene = result.Result;
                    GroupModel.Clear();
                    if (scene != null)
                    {
                        if (scene.Root != null)
                        {
                            foreach (var node in scene.Root.Traverse())
                            {
                                if (node is MaterialGeometryNode m)
                                {
                                    if (m.Material is PBRMaterialCore pbr)
                                    {
                                    }
                                    else if(m.Material is PhongMaterialCore phong)
                                    {
                                    }
                                }
                            }
                        }
                        GroupModel.AddNode(scene.Root);
                        foreach(var n in scene.Root.Traverse())
                        {
                            n.Tag = new AttachedNodeViewModel(n);
                        }
                    }                  
                }
                else if (result.IsFaulted && result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void StartAnimation()
        {
            compositeHelper.Rendering += CompositeHelper_Rendering;
        }


        private void CompositeHelper_Rendering(object sender, System.Windows.Media.RenderingEventArgs e)
        {
        }

        private string OpenFileDialog(string filter)
        {
            var d = new OpenFileDialog();
            d.CustomPlaces.Clear();

            d.Filter = filter;

            if (!d.ShowDialog().Value)
            {
                return null;
            }

            return d.FileName;
        }

    }

}