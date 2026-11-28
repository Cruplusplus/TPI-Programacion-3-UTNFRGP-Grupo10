<%@ Page Title="Informes y Estadísticas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Informes.aspx.cs" Inherits="TPINT_GRUPO_10_PR3.Vistas.Informes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/Informes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="informes-wrapper">

        <!-- 1. INFORME DE ASISTENCIA GENERAL -->
        <section class="informe-card">
            <h3>Informe de asistencia general</h3>
            <p class="informe-subtitulo">Consulta las estadísticas generales de asistencia durante un período determinado.</p>

            <div class="informe-filtros-row">
                <div class="filtro-campo">
                    <label for="<%= txtFechaDesdeGen.ClientID %>">Fecha Desde:</label>
                    <asp:TextBox ID="txtFechaDesdeGen" runat="server" TextMode="Date" CssClass="filtro-input"></asp:TextBox>
                </div>

                <div class="filtro-campo">
                    <label for="<%= txtFechaHastaGen.ClientID %>">Fecha Hasta:</label>
                    <asp:TextBox ID="txtFechaHastaGen" runat="server" TextMode="Date" CssClass="filtro-input"></asp:TextBox>
                </div>

                <div class="filtro-btn-col">
                    <asp:Button ID="btnGenerarGen" runat="server" Text="Generar Informe" CssClass="btn-generar" OnClick="btnGenerarGen_Click" />
                </div>
            </div>

            <asp:Label ID="lblMensajeGen" runat="server" CssClass="informe-mensaje"></asp:Label>

            <hr class="informe-divisor" />

            <h4 class="resultados-titulo">Resultados de asistencia</h4>
            <div class="resultados-row">
                <div class="resultado-caja">
                    <asp:Label ID="lblTotalGen" runat="server" Text="Total de turnos: -"></asp:Label>
                </div>
                <div class="resultado-caja">
                    <asp:Label ID="lblPresentesGen" runat="server" Text="Presentes: -"></asp:Label>
                </div>
                <div class="resultado-caja">
                    <asp:Label ID="lblAusentesGen" runat="server" Text="Ausentes: -"></asp:Label>
                </div>
                <div class="resultado-caja">
                    <asp:Label ID="lblPendientesGen" runat="server" Text="Pendientes: -"></asp:Label>
                </div>
            </div>
        </section>

        <!-- 2. INFORME DE ASISTENCIA POR MÉDICO -->
        <section class="informe-card">
            <h3>Informe de asistencia por médico</h3>
            <p class="informe-subtitulo">Consulta las estadísticas de asistencia de un profesional en particular.</p>

            <div class="informe-filtros-row">
                <div class="filtro-campo filtro-medico">
                    <label for="<%= ddlMedicos.ClientID %>">Médico:</label>
                    <asp:DropDownList ID="ddlMedicos" runat="server" CssClass="filtro-input">
                    </asp:DropDownList>
                </div>

                <div class="filtro-campo">
                    <label for="<%= txtFechaDesdeMed.ClientID %>">Fecha Desde:</label>
                    <asp:TextBox ID="txtFechaDesdeMed" runat="server" TextMode="Date" CssClass="filtro-input"></asp:TextBox>
                </div>

                <div class="filtro-campo">
                    <label for="<%= txtFechaHastaMed.ClientID %>">Fecha Hasta:</label>
                    <asp:TextBox ID="txtFechaHastaMed" runat="server" TextMode="Date" CssClass="filtro-input"></asp:TextBox>
                </div>

                <div class="filtro-btn-col">
                    <asp:Button ID="btnGenerarMed" runat="server" Text="Generar Informe" CssClass="btn-generar" OnClick="btnGenerarMed_Click" />
                </div>
            </div>

            <asp:Label ID="lblMensajeMed" runat="server" CssClass="informe-mensaje"></asp:Label>

            <hr class="informe-divisor" />

            <h4 class="resultados-titulo">Resultados de asistencia</h4>
            <div class="resultados-row">
                <div class="resultado-caja">
                    <asp:Label ID="lblTotalMed" runat="server" Text="Total de turnos: -"></asp:Label>
                </div>
                <div class="resultado-caja">
                    <asp:Label ID="lblPresentesMed" runat="server" Text="Presentes: -"></asp:Label>
                </div>
                <div class="resultado-caja">
                    <asp:Label ID="lblAusentesMed" runat="server" Text="Ausentes: -"></asp:Label>
                </div>
                <div class="resultado-caja">
                    <asp:Label ID="lblPendientesMed" runat="server" Text="Pendientes: -"></asp:Label>
                </div>
            </div>
        </section>

    </div>
</asp:Content>
