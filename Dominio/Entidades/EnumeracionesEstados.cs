using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class EnumeracionesEstados
    {
        public enum TipoSuperficie
        {Ladrillo,Cemento,Pasto}

        public enum ModalidadTorneo
        {Single,Pareja}

        public enum CategoriaTorneo
        {Masculino,Femenino}
        public enum EstadoTorneo
        { Planificado, InscripcionAbierta, EnCurso, Finalizado, Cancelado}

        public enum EstadoPartido
        { Programado, EnJuego, Finalizado, Suspendido }
    }
}
