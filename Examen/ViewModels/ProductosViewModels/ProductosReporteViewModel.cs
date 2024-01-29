﻿using Examen.Repositories;
using Examen.Repositories.IRepositories;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Examen.ViewModels.ProductosViewModels
{
    class ProductosReporteViewModel : ViewModelBase
    {
        // Porpiedades.
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private string _errorMessageFechaInicio;
        private string _errorMessageFechaFin;
        private StiReport _stimulsoftReport;

        private IProductosRepository _productosRepository;

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set
            {
                _fechaInicio = value;
                OnPropertyChanged(nameof(FechaInicio));
            }
        }

        public DateTime FechaFin
        {
            get => _fechaFin;
            set
            {
                _fechaFin = value;
                OnPropertyChanged(nameof(FechaFin));
            }
        }

        public string ErrorMessageFechaInicio
        {
            get => _errorMessageFechaInicio;
            set
            {
                _errorMessageFechaInicio = value;
                OnPropertyChanged(nameof(ErrorMessageFechaInicio));
            }
        }

        public string ErrorMessageFechaFin
        {
            get => _errorMessageFechaFin;
            set
            {
                _errorMessageFechaFin = value;
                OnPropertyChanged(nameof(ErrorMessageFechaFin));
            }
        }

        public StiReport StimulsoftReport
        {
            get => _stimulsoftReport;
            set
            {
                _stimulsoftReport = value;
                OnPropertyChanged(nameof(StimulsoftReport));
            }
        }

        // Commandos.
        public ICommand GenerarReporteDetalladoCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor.
        public ProductosReporteViewModel()
        {
            _productosRepository = new ProductosRepository();

            //Inicializacion de comandos.
            GenerarReporteDetalladoCommand = new RelayCommandModel(ExecuteGenerarReporteDetalladoCommand, CanExecuteGenerarReporteDetalladoCommand);
            CancelCommand = new RelayCommandModel(ExecuteCancelCommand);

            //Inicializacion del reporte.
            StimulsoftReport = new StiReport();

            // Establece la fecha de inicio y fin como la fecha actual.
            FechaInicio = DateTime.Now;
            FechaFin = DateTime.Now;
        }

        //Metodos.

        // Generar reporte.
        private void ExecuteGenerarReporteDetalladoCommand(object obj)
        {

            if (CanExecuteGenerarReporteDetalladoCommand(null))
            {
                try
                {

                    ErrorMessageFechaInicio = string.Empty;
                    ErrorMessageFechaFin = string.Empty;

                    var reporteProducto = _productosRepository.GetAllProductos();

                    if (reporteProducto.Any())
                    {
                        StimulsoftReport.Load("C:\\Users\\leand\\OneDrive\\Escritorio\\ReportedeProductos.mrt");

                        StimulsoftReport.RegData("Detalle de Productos", reporteProducto.Select(p => new 
                        {
                            ID = p.ID,
                            Nombre = p.Nombre,
                            Precio = p.Precio,
                            Categoria = p.Categoria
                        }));

                        _stimulsoftReport.Show();
                    }
                    else
                    {
                        ErrorMessageFechaInicio = "No hay datos disponibles para el informe detallado.";
                        ErrorMessageFechaFin = "No hay datos disponibles para el informe detallado.";
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ErrorMessageFechaInicio = "Error al generar el informe detallado: " + ex.Message;
                    ErrorMessageFechaFin = "Error al generar el informe detallado: " + ex.Message;
                }
                catch (Exception ex)
                {
                    ErrorMessageFechaInicio = "Error al generar el informe detallado: " + ex.Message;
                    ErrorMessageFechaFin = "Error al generar el informe detallado: " + ex.Message;
                }
            }
        }

        private bool CanExecuteGenerarReporteDetalladoCommand(object obj)
        {

            if (FechaInicio == default || FechaFin == default)
            {
                ErrorMessageFechaInicio = "Ambas fechas deben estar establecidas.";
                ErrorMessageFechaFin = "Ambas fechas deben estar establecidas.";
                return false;
            }

            if (FechaInicio > FechaFin)
            {
                ErrorMessageFechaInicio = "La fecha de inicio debe ser anterior a la fecha de fin.";
                ErrorMessageFechaFin = "La fecha de fin debe ser posterior a la fecha de inicio.";
                return false;
            }

            ErrorMessageFechaInicio = string.Empty;
            ErrorMessageFechaFin = string.Empty;

            return true;
        }

        // Cancelar.
        private void ExecuteCancelCommand(object obj)
        {
            // Limpia cualquier mensaje de error
            ErrorMessageFechaFin = string.Empty;
            ErrorMessageFechaInicio = string.Empty;
        }
    }
}
