<%@ Page Title="ABML de Pacientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMLPacientes.aspx.cs" Inherits="TPINT_GRUPO_10_PR3.ABMLPacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/ABMLPacientes.css" rel="stylesheet" type="text/css" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="abml-container">

        <!-- FORMULARIO DE CARGA / EDICIÓN -->
        <section class="card">
            <h3><asp:Label ID="lblTituloForm" runat="server" Text="Alta de Paciente"></asp:Label></h3>

            <div class="form-grid">
                <div class="form-group">
                    <label>DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ej: 38456123"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Apellido"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Sexo</label>
                    <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- Seleccione --" Value="" />
                        <asp:ListItem Text="Masculino" Value="Masculino" />
                        <asp:ListItem Text="Femenino" Value="Femenino" />
                        <asp:ListItem Text="Otro" Value="Otro" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Nacionalidad</label>
                    <asp:TextBox ID="txtNacionalidad" runat="server" CssClass="form-control" placeholder="Ej: Argentina"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Fecha de Nacimiento</label>
                    <asp:TextBox ID="txtFechaNac" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" placeholder="Calle y altura"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Provincia</label>
                    <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Localidad</label>
                    <asp:DropDownList ID="ddlLocalidad" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="correo@ejemplo.com"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej: 11-1234-5678"></asp:TextBox>
                </div>
            </div>

            <div class="form-actions">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
                <asp:Label ID="lblMensaje" runat="server" Font-Bold="true"></asp:Label>
            </div>
        </section>

        <!-- GRILLA CON BÚSQUEDA, FILTRO Y PAGINACIÓN -->
        <section class="card">
            <h3>Listado de Pacientes</h3>

            <div class="filter-bar">
                <div class="form-group" style="flex: 2; min-width: 200px;">
                    <label>Buscar (DNI, Nombre o Apellido)</label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar..."></asp:TextBox>
                </div>
                <div class="form-group" style="flex: 1; min-width: 160px;">
                    <label>Provincia</label>
                    <asp:DropDownList ID="ddlFiltroProvincia" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group" style="flex: 1; min-width: 140px;">
                    <label>Sexo</label>
                    <asp:DropDownList ID="ddlFiltroSexo" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Todos" Value="" />
                        <asp:ListItem Text="Masculino" Value="Masculino" />
                        <asp:ListItem Text="Femenino" Value="Femenino" />
                        <asp:ListItem Text="Otro" Value="Otro" />
                    </asp:DropDownList>
                </div>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
                <asp:Button ID="btnLimpiarFiltro" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiarFiltro_Click" />
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvPacientes" runat="server" AutoGenerateColumns="False"
                    CssClass="grid-pacientes" AllowPaging="True" PageSize="5"
                    OnPageIndexChanging="gvPacientes_PageIndexChanging"
                    OnRowCommand="gvPacientes_RowCommand"
                    EmptyDataText="No hay pacientes para mostrar.">
                    <Columns>
                        <asp:BoundField DataField="DNI" HeaderText="DNI" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Sexo" HeaderText="Sexo" />
                        <asp:BoundField DataField="FechaNacimiento" HeaderText="Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Provincia" HeaderText="Provincia" />
                        <asp:BoundField DataField="Localidad" HeaderText="Localidad" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" Text="Modificar" CommandName="Modificar"
                                    CommandArgument='<%# Eval("DNI") %>' CssClass="btn btn-edit" />
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar"
                                    CommandArgument='<%# Eval("DNI") %>' CssClass="btn btn-delete"
                                    OnClientClick="return confirm('¿Eliminar este paciente?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="grid-pager" />
                </asp:GridView>
            </div>
        </section>

    </div>
</asp:Content>