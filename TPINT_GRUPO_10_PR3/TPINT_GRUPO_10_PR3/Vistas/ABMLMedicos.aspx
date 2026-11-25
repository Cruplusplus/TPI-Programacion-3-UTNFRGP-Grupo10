<%@ Page Title="ABML de Médicos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMLMedicos.aspx.cs" Inherits="TPINT_GRUPO_10_PR3.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/ABMLMedicos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="medicos-container">
        <h2>ABML de Médicos</h2>
        <hr />

        <!-- FORMULARIO DE CARGA / EDICION -->
        <section class="card">
            <h3>
                <asp:Label ID="lblTituloForm" runat="server" Text="Datos del Médico"></asp:Label></h3>

            <div class="form-grid">
                <!-- Identificacion y Datos Personales -->
                <div class="form-group">
                    <label>Legajo:</label>
                    <asp:TextBox ID="txtLegajo" runat="server" CssClass="form-control" MaxLength="15" placeholder="Ej: MED001"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLegajo" runat="server" ControlToValidate="txtLegajo" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>DNI:</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ej: 35123456"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDni" runat="server" ControlToValidate="txtDni" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Nombre:</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Apellido:</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Apellido"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Sexo:</label>
                    <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- Seleccione --" Value="" />
                        <asp:ListItem Text="Masculino" Value="M" />
                        <asp:ListItem Text="Femenino" Value="F" />
                        <asp:ListItem Text="Otro" Value="X" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvSexo" runat="server" ControlToValidate="ddlSexo" InitialValue="" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Nacionalidad:</label>
                    <asp:TextBox ID="txtNacionalidad" runat="server" CssClass="form-control" placeholder="Ej: Argentina"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNacionalidad" runat="server" ControlToValidate="txtNacionalidad" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Fecha de Nacimiento:</label>
                    <asp:TextBox ID="txtFechaNac" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFechaNac" runat="server" ControlToValidate="txtFechaNac" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Dirección:</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" placeholder="Calle y altura"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDireccion" runat="server" ControlToValidate="txtDireccion" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Provincia:</label>
                    <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProvincia" runat="server" ControlToValidate="ddlProvincia" InitialValue="" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Localidad:</label>
                    <asp:DropDownList ID="ddlLocalidad" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvLocalidad" runat="server" ControlToValidate="ddlLocalidad" InitialValue="" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Correo Electrónico:</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email" placeholder="correo@ejemplo.com"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ControlToValidate="txtCorreo" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Teléfono:</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej: 11-1234-5678"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label>Especialidad:</label>
                    <asp:DropDownList ID="ddlEspecialidades" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server" ControlToValidate="ddlEspecialidades" InitialValue="" ErrorMessage="Campo obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <!-- Dias y Horarios de Atencion -->
            <div class="horarios-section">
                <h4>Días y Horarios de Atención</h4>
                <div class="form-group">
                    <label>Días de Atención:</label>
                    <asp:CheckBoxList ID="cblDiasAtencion" runat="server" RepeatDirection="Horizontal" CssClass="dias-checkboxlist">
                        <asp:ListItem Text="Domingo" Value="0" />
                        <asp:ListItem Text="Lunes" Value="1" />
                        <asp:ListItem Text="Martes" Value="2" />
                        <asp:ListItem Text="Miércoles" Value="3" />
                        <asp:ListItem Text="Jueves" Value="4" />
                        <asp:ListItem Text="Viernes" Value="5" />
                        <asp:ListItem Text="Sábado" Value="6" />
                    </asp:CheckBoxList>
                </div>

                <div class="form-grid horarios-grid">
                    <div class="form-group">
                        <label>Hora Inicio:</label>
                        <asp:DropDownList ID="ddlHoraInicio" runat="server" CssClass="form-control">
                            <asp:ListItem Text="08:00" Value="08:00" />
                            <asp:ListItem Text="09:00" Value="09:00" />
                            <asp:ListItem Text="10:00" Value="10:00" />
                            <asp:ListItem Text="11:00" Value="11:00" />
                            <asp:ListItem Text="12:00" Value="12:00" />
                            <asp:ListItem Text="13:00" Value="13:00" />
                            <asp:ListItem Text="14:00" Value="14:00" />
                            <asp:ListItem Text="15:00" Value="15:00" />
                            <asp:ListItem Text="16:00" Value="16:00" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Hora Fin:</label>
                        <asp:DropDownList ID="ddlHoraFin" runat="server" CssClass="form-control">
                            <asp:ListItem Text="12:00" Value="12:00" />
                            <asp:ListItem Text="13:00" Value="13:00" />
                            <asp:ListItem Text="14:00" Value="14:00" />
                            <asp:ListItem Text="15:00" Value="15:00" />
                            <asp:ListItem Text="16:00" Value="16:00" />
                            <asp:ListItem Text="17:00" Value="17:00" />
                            <asp:ListItem Text="18:00" Value="18:00" />
                            <asp:ListItem Text="19:00" Value="19:00" />
                            <asp:ListItem Text="20:00" Value="20:00" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <!-- Credenciales de Acceso -->
            <div class="credenciales-section">
                <h4>Credenciales de Acceso al Sistema</h4>
                <div class="form-grid">
                    <div class="form-group">
                        <label>Usuario:</label>
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Nombre de usuario"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ErrorMessage="Obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label>Contraseña:</label>
                        <asp:TextBox ID="txtContrasenia" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtContrasenia" ErrorMessage="Obligatorio" ForeColor="Red" ValidationGroup="GrupoMedico">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label>Repetir Contraseña:</label>
                        <asp:TextBox ID="txtRepetirContrasenia" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repetir contraseña"></asp:TextBox>
                        <asp:CompareValidator ID="cvPass" runat="server" ControlToCompare="txtContrasenia" ControlToValidate="txtRepetirContrasenia" ErrorMessage="Las contraseñas no coinciden" ForeColor="Red" ValidationGroup="GrupoMedico">Las contraseñas no coinciden</asp:CompareValidator>
                    </div>
                </div>
            </div>

            <!-- Acciones -->
            <div class="form-actions">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Médico" CssClass="btn btn-primary" OnClick="btnGuardar_Click" ValidationGroup="GrupoMedico" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnLimpiar_Click" />
                <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-feedback"></asp:Label>
            </div>
        </section>

        <!-- BUSCADOR Y GRILLA -->
        <section class="card" style="margin-top: 24px;">
            <h3>Listado de Médicos Activos</h3>

            <div class="filter-bar">
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" Placeholder="Buscar por apellido, nombre o legajo..."></asp:TextBox>
                <asp:DropDownList ID="ddlFiltroEspecialidad" runat="server" CssClass="form-control">
                </asp:DropDownList>

                <asp:DropDownList ID="ddlFiltroDia" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Todos los días --" Value="-1" />
                    <asp:ListItem Text="Domingo" Value="0" />
                    <asp:ListItem Text="Lunes" Value="1" />
                    <asp:ListItem Text="Martes" Value="2" />
                    <asp:ListItem Text="Miércoles" Value="3" />
                    <asp:ListItem Text="Jueves" Value="4" />
                    <asp:ListItem Text="Viernes" Value="5" />
                    <asp:ListItem Text="Sábado" Value="6" />
                </asp:DropDownList>

                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar (LIKE)" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnFiltrar_Click" />
                <asp:Button ID="btnMostrarTodos" runat="server" Text="Mostrar Todos" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnMostrarTodos_Click" />
            </div>

            <div class="table-responsive">
                <asp:GridView ID="GridViewMedicos" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="5"
                    CssClass="grid-medicos" OnPageIndexChanging="GridViewMedicos_PageIndexChanging" OnRowCommand="GridViewMedicos_RowCommand"
                    OnRowDataBound="GridViewMedicos_RowDataBound" DataKeyNames="IdPersona">
                    <Columns>
                        <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                        <asp:BoundField DataField="Dni" HeaderText="DNI" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                        <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                        <asp:BoundField DataField="DiasAtencion" HeaderText="Días de Atención" />
                        <asp:BoundField DataField="Horario" HeaderText="Horario" />
                        <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                        <asp:ButtonField ButtonType="Button" CommandName="EventoEditar" Text="Editar" ControlStyle-CssClass="btn btn-secondary" />
                        <asp:ButtonField ButtonType="Button" CommandName="EventoEliminar" Text="Eliminar (Baja Lógica)" ControlStyle-CssClass="btn btn-delete" />
                    </Columns>
                    <PagerStyle CssClass="grid-pager" />
                </asp:GridView>
            </div>
        </section>
    </div>
</asp:Content>
